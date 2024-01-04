using System;
using System.Security.Claims;
using FisCompendium.Data.Game_Data;
using FisCompendium.Data.System_Data;
using FisCompendium.Data.User_Data;
using FisCompendium.Data.Utility;
using FisCompendium.Repository;
using FisCompendium.Web.Utilities.GameData.Knowledge;
using FisCompendium.Web.Utilities.SystemData;
using FisCompendium.Web.Utilities.UserData;
using FisCompendium.Web.Utilities.UserData.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FisCompendium.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailProvider>(provider =>
            {
                var exceptionLogRepository = provider.GetService<IRepository<ExceptionLog>>();
                return new EmailProvider(exceptionLogRepository);
            });

            services.AddTransient(provider => RepositoryFactory.Create<KnowledgeItem>(DatabaseSelector.GameData));
            services.AddTransient(provider => RepositoryFactory.Create<KnowledgeItemComment>(DatabaseSelector.GameData));
            services.AddTransient(provider => RepositoryFactory.Create<KnowledgeItemLog>(DatabaseSelector.GameData));

            services.AddTransient(provider => RepositoryFactory.Create<UserLoginData>(DatabaseSelector.PlayerData));
            services.AddTransient(provider => RepositoryFactory.Create<UserAuthData>(DatabaseSelector.PlayerData));
            services.AddTransient(provider => RepositoryFactory.Create<UserPermissions>(DatabaseSelector.PlayerData));
            services.AddTransient(provider => RepositoryFactory.Create<UserPoints>(DatabaseSelector.PlayerData));
            services.AddTransient(provider => RepositoryFactory.Create<PlayerLogEntry>(DatabaseSelector.PlayerData));
            services.AddTransient(provider => RepositoryFactory.Create<SystemLog>(DatabaseSelector.PlayerData));
            services.AddTransient(provider => RepositoryFactory.Create<UserRecoveryToken>(DatabaseSelector.PlayerData));

            services.AddTransient(provider => RepositoryFactory.Create<QMNote>(DatabaseSelector.QMGameData));
            services.AddTransient(provider => RepositoryFactory.Create<QMNoteLog>(DatabaseSelector.QMGameData));

            services.AddTransient(provider => RepositoryFactory.Create<ExceptionLog>(DatabaseSelector.System));
            services.AddTransient(provider => RepositoryFactory.Create<SystemLock>(DatabaseSelector.System));
            services.AddTransient(provider => RepositoryFactory.Create<ConfigItem>(DatabaseSelector.System));

            services.AddTransient<IAccountActionExecutor>(provider =>
            {
                var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                var userLoginDataRepository = provider.GetService<IRepository<UserLoginData>>();
                var userAuthDataRepository = provider.GetService<IRepository<UserAuthData>>();
                var userPermissionsRepository = provider.GetService<IRepository<UserPermissions>>();
                return new AccountActionExecutor(httpContextAccessor, userLoginDataRepository, userAuthDataRepository, userPermissionsRepository);
            });
            services.AddTransient<IAccountRecovery>(provider =>
            {
                var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                var userLoginDataRepository = provider.GetService<IRepository<UserLoginData>>();
                var userAuthDataRepository = provider.GetService<IRepository<UserAuthData>>();
                var userRecoveryTokenRepository = provider.GetService<IRepository<UserRecoveryToken>>();
                var emailProvider = provider.GetService<IEmailProvider>();
                return new AccountRecovery(httpContextAccessor, userLoginDataRepository, userAuthDataRepository, userRecoveryTokenRepository, emailProvider);
            });
            services.AddTransient<IUserPermissionLookup>(provider =>
            {
                var userLoginDataRepository = provider.GetService<IRepository<UserLoginData>>();
                var userPermissionsRepository = provider.GetService<IRepository<UserPermissions>>();
                return new UserPermissionLookup(userLoginDataRepository, userPermissionsRepository);
            });
            services.AddTransient<IUserIdentityAuthenticator>(provider =>
            {
                var accountActionExecutor = provider.GetService<IAccountActionExecutor>();
                var userPermissionLookup = provider.GetService<IUserPermissionLookup>();
                var systemLockRepository = provider.GetService<ISystemLockRepository>();
                return new UserIdentityAuthenticator(accountActionExecutor, userPermissionLookup, systemLockRepository);
            });
            services.AddTransient<IPlayerPointsRepository>(provider =>
            {
                var userPointsRepository = provider.GetService<IRepository<UserPoints>>();
                var playerLogRepository = provider.GetService<IRepository<PlayerLogEntry>>();
                var userLoginDataRepository = provider.GetService<IRepository<UserLoginData>>();
                return new PlayerPointsRepository(userPointsRepository, playerLogRepository, userLoginDataRepository);
            });
            services.AddTransient<IKnowledgeRepository>(provider =>
            {
                var knowledgeItemRepository = provider.GetService<IRepository<KnowledgeItem>>();
                var commentsRepository = provider.GetService<IRepository<KnowledgeItemComment>>();
                var knowledgeItemLogRepository = provider.GetService<IRepository<KnowledgeItemLog>>();
                return new KnowledgeRepository(knowledgeItemRepository, commentsRepository, knowledgeItemLogRepository);
            });
            services.AddTransient<IQMNoteRepository>(provider =>
            {
                var qmNotesRepository = provider.GetService<IRepository<QMNote>>();
                var logRepository = provider.GetService<IRepository<QMNoteLog>>();
                return new QMNotesRepository(qmNotesRepository, logRepository);
            });
            services.AddTransient<ISystemLockRepository>(provider =>
            {
                var repository = provider.GetService<IRepository<SystemLock>>();
                return new SystemLockRepository(repository);
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(186);
            });

            // FIXME: dotnet run + this = redirects from correct port to 5001 which doesn't serve anything
            // services.AddHttpsRedirection(options =>
            // {
            //     options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            //     options.HttpsPort = 5001;
            // });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsQM", policy => policy.RequireRole(PermissionsLevel.QM.ToString()));
                options.AddPolicy("IsTrustedPlayer", policy => policy.RequireRole(PermissionsLevel.TrustedPlayer.ToString()));
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            //app.UseCors("AllowScreamingInKagome");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'none'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; connect-src 'self'; font-src 'self'; object-src 'self'; media-src 'self'; child-src 'self'; form-action 'self'; frame-ancestors 'none'; base-uri 'self'");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                await next();
            });

            app.UseStatusCodePages();

            app.Use(serviceProvider.GetService<IUserIdentityAuthenticator>().GetUserIdentity);

            //Deny access to protected static files
            app.Use(async (context, next) =>
            {
                var isProtected = context.Request.Path.StartsWithSegments("/protected");
                var isQM = context.User.HasClaim(ClaimTypes.Role, PermissionsLevel.QM.ToString());

                if (isProtected && !isQM)
                {
                    context.Response.StatusCode = 403;
                }
                else
                {
                    try
                    {
                        await next.Invoke();
                    }
                    catch (InvalidOperationException e)
                    {
                        if (e.Message == "No authenticationScheme was specified, and there was no DefaultChallengeScheme found.")
                        {
                            context.Response.StatusCode = 403;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            });
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                await next();
            });
        }
    }
}
