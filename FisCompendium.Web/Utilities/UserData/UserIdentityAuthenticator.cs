using System;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FisCompendium.Data.User_Data;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.Account;
using FisCompendium.Web.Utilities.SystemData;
using FisCompendium.Web.Utilities.UserData.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FisCompendium.Web.Utilities.UserData
{
    public class UserIdentityAuthenticator : IUserIdentityAuthenticator
    {
        public IAccountActionExecutor AccountActionExecutor { get; }
        public IUserPermissionLookup UserPermissionLookup { get; }
        public ISystemLockRepository SystemLockRepository { get; }

        public UserIdentityAuthenticator(IAccountActionExecutor accountActionExecutor, IUserPermissionLookup userPermissionLookup, ISystemLockRepository systemLockRepository)
        {
            AccountActionExecutor = accountActionExecutor;
            UserPermissionLookup = userPermissionLookup;
            SystemLockRepository = systemLockRepository;
        }

        public Task GetUserIdentity(HttpContext httpContext, Func<Task> next)
        {
            var systemLocked = SystemLockRepository.GetIsSystemLocked();
            if (systemLocked) return LockedPage(httpContext);

            var userName = httpContext.GetCurrentUser();

            //var userIPEntry = new UserIPEntry
            //{
            //    UserName = userName ?? "Guest",
            //    UserIP = httpContext.Connection.RemoteIpAddress.ToString(),
            //    LastSeen = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            //};

            //if (userName != "Guest")
            //{
            //    var isBanned = AccountActionExecutor.LogUserIP(userIPEntry);
            //    if (isBanned) return BannedPage(httpContext);
            //}

            var authTicket = httpContext.GetUserAuthTicket();
            var authenticationAction = new AccountAuthenticate
            {
                Username = userName,
                AuthTicket = authTicket
            };

            var authenticated = !string.IsNullOrWhiteSpace(userName)
                && !string.IsNullOrWhiteSpace(authTicket)
                && AccountActionExecutor.TryAuthenticate(authenticationAction);
            if (authenticated)
            {
                //If player is a QM, also add "trusted player" role
                var userRole = UserPermissionLookup.GetPermissionsForUser(userName);
                if (userRole == PermissionsLevel.QM)
                {
                    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, PermissionsLevel.QM.ToString()),
                        new Claim(ClaimTypes.Role, PermissionsLevel.TrustedPlayer.ToString())
                    }));
                }
                else
                {
                    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, userRole.ToString())
                    }));
                }
            }
            else
            {
                httpContext.User = new ClaimsPrincipal(new ClaimsIdentity[0]);
            }

            return next();
        }

        private static async Task BannedPage(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "text/html";
            const string msg = "<h1 style='color:red'>You have been banned. &#x1F528;</h1>";
            var buffer = Encoding.UTF8.GetBytes(msg);
            context.Response.ContentLength = buffer.Length;

            using (var stream = context.Response.Body)
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }
        }

        private static async Task LockedPage(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "text/html";
            const string msg = "<h1 style='color:red'>The site has been temporarily locked. &#x1F512;</h1>";
            var buffer = Encoding.UTF8.GetBytes(msg);
            context.Response.ContentLength = buffer.Length;

            using (var stream = context.Response.Body)
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }
        }
    }
}
