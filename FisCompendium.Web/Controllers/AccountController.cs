using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FisCompendium.Data.User_Data;
using FisCompendium.Data.Utility;
using FisCompendium.Repository;
using FisCompendium.Utility;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.Account;
using FisCompendium.Web.Utilities.UserData;
using FisCompendium.Web.Utilities.UserData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FisCompendium.Web.Controllers
{
    public class AccountController : Controller
    {
        private IAccountActionExecutor AccountActionExecutor { get; }
        private IAccountRecovery AccountRecovery { get; }
        private IRepository<UserLoginData> UserLoginDataRepository { get; }
        private IRepository<SystemLog> SystemLogRepository { get; }

        public AccountController
        (
            IAccountActionExecutor accountActionExecutor,
            IAccountRecovery accountRecovery,
            IRepository<UserLoginData> userLoginDataRepository,
            IRepository<SystemLog> systemLogRepository
        )
        {
            AccountActionExecutor = accountActionExecutor;
            AccountRecovery = accountRecovery;
            UserLoginDataRepository = userLoginDataRepository;
            SystemLogRepository = systemLogRepository;
        }

        public IActionResult Error() => View();
        private IActionResult RedirectToLast()
        {
            var lastPage = TempData.GetRedirect();
            if (lastPage == "/" || lastPage == "//") return RedirectToAction("Index", "Home");

            return lastPage != null && (!lastPage.Contains("Account") || lastPage.Contains("UpdatePreferences")) && !lastPage.Contains("Error") ? (IActionResult)Redirect(lastPage) : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string lastPage = null)
        {
            if (lastPage != null && lastPage != "/")
            {
                TempData.SetRedirect(lastPage);
            }

            if (User.Identity?.Name != null)
            {
                return RedirectToLast();
            }

            return User?.Identity?.IsAuthenticated == true ? RedirectToLast() : View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegistrationRaw data)
        {
            if (!string.IsNullOrEmpty(data.ConfirmHuman))
            {
                LogActivity("Failed honeypot check on login", data);
                //return RedirectToAction("TAC", "Home");
                return StatusCode(404);
            }

            if (data.Captcha == null || data.Captcha.ToLower() != "into")
            {
                LogActivity("Failed captcha check on login", data);
                return StatusCode(404);
            }

            if (User.Identity?.Name != null)
            {
                return RedirectToLast();
            }

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            var model = AccountRegistration.FromRaw(data);

            var userRegistered = AccountActionExecutor.TryExecute(model);
            if (userRegistered)
            {
                TempData.AddMessage($"Welcome to the site, {model.Username}!");
                return RedirectToLast();
            }

            TempData.AddError("That username is already taken.");
            return View();
        }

        [HttpGet]
        public IActionResult Login(string lastPage = null)
        {
            if (lastPage != null && lastPage != "/")
            {
                TempData.SetRedirect(lastPage);
            }

            return User?.Identity?.IsAuthenticated == true ? RedirectToLast() : View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginRaw data)
        {
            if (!string.IsNullOrEmpty(data.ConfirmHuman))
            {
                LogActivity("Failed honeypot check on login", data);
                return RedirectToAction("TAC", "Home");
            }

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            var model = AccountLogin.FromRaw(data);

            var userLoggedIn = AccountActionExecutor.TryExecute(model);
            if (userLoggedIn) return RedirectToLast();

            TempData.AddError("Invalid login attempt.");
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult Logout()
        {
            var currentUser = User?.Identity?.Name;
            if (currentUser == null) return RedirectToAction("Index", "Home");

            AccountActionExecutor.Logout(currentUser);

            TempData.AddMessage("You have been logged out.");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(AccountChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            var passwordChanged = AccountActionExecutor.TryExecute(model);
            if (passwordChanged)
            {
                TempData.AddMessage("Password has been updated.");
                return RedirectToLast();
            }

            TempData.AddError("Username or password is invalid.");
            return View();
        }

        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeEmail(AccountChangeEmail model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            var changeEmailResult = AccountActionExecutor.TryExecute(model);
            if (changeEmailResult)
            {
                TempData.AddMessage("Recovery email has been updated.");
                return RedirectToLast();
            }

            TempData.AddError("Username or password is invalid.");
            return View();
        }

        [HttpGet]
        public IActionResult UpdatePreferences()
        {
            var userName = User?.Identity?.Name;
            var loginData = UserLoginDataRepository.GetWhere(x => x.UserName == userName).FirstOrDefault();

            if (loginData == null)
            {
                TempData.SetRedirect("/Account/UpdatePreferences");
                TempData.AddError("You must login first.");
                return RedirectToAction("Login");
            }

            var subscribed = loginData?.Subscribed ?? false;

            var model = new AccountUpdatePreferences
            {
                Username = userName,
                Subscribed = subscribed,
                UselessCheckboxPreference = loginData.UselessCheckboxPreference,
                RedditUsername = loginData.RedditUsername
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePreferences(AccountUpdatePreferences model)
        {
            model.Username = User?.Identity?.Name;

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View(model);
            }

            var changePreferencesResult = AccountActionExecutor.TryExecute(model);
            if (changePreferencesResult)
            {
                TempData.AddMessage("Preferences have been updated.");
                return View(model);
            }

            TempData.AddError("I don't know how, but you managed to break the form. Try again?");
            return View(model);
        }

        [HttpGet]
        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordRecovery(AccountPasswordRecovery model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            try
            {
                AccountRecovery.TryInitiateRecovery(model);
            }
            catch (SmtpException e)
            {
                TempData.AddError("There was an issue sending the recovery email. Please contact the site administrator with the following: \"CODE JABBERWOCKY\".");
                return View("Recover");
            }

            //Show message regardless of success
            TempData.AddMessage("If the given email is valid for the specified user, a recovery email has been sent to that address. Please allow a couple minutes for the email to arrive.");
            return View("Recover");
        }

        [HttpGet]
        public IActionResult Recover()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recover(AccountRecover model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            var passwordRecovered = AccountRecovery.TryRecover(model);
            if (passwordRecovered)
            {
                //Go ahead and submit a login request
                var loginRequest = new AccountLogin
                {
                    Username = model.Username,
                    Password = model.Password
                };
                var loginSucceeded = AccountActionExecutor.TryExecute(loginRequest);

                if (loginSucceeded)
                {
                    TempData.AddMessage("Your password has been updated.");
                    return RedirectToLast();
                }
            }

            TempData.AddError("Recovery attempt failed or ticket has expired.");
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateStrongPassword(string password)
        {
            if (CommonPasswords.Contains(password))
            {
                return Json("This password is in a publicly accessible list of common passwords.");
            }

            password = password ?? "";

            var passwordStrength = 0.0;
            foreach (var c in password)
            {
                if (char.IsDigit(c)) passwordStrength += 3.322;
                else if (char.IsLower(c)) passwordStrength += 4.7;
                else if (char.IsUpper(c)) passwordStrength += 4.7;
                else passwordStrength += 5.04;
            }

            return passwordStrength >= 28 ? Json(true) : Json($"Minimum password strength: {Math.Round(passwordStrength, 1)}/28.0");
        }

        private void LogActivity(string details, dynamic data)
        {
            var logEntry = new SystemLog
            {
                Username = User.Identity?.Name ?? "Guest",
                IP = HttpContext.Connection.RemoteIpAddress.ToString(),
                Details = $"{DateTime.Now.ToString()}: {details}",
                Data = JsonConvert.SerializeObject(data)
            };

            SystemLogRepository.Upsert(logEntry);
        }

        public IActionResult LinkRedditAccount()
        {
            //Generate a request identifier
            var requestIdentifier = RNG.Next(0, int.MaxValue).ToString();

            var userName = User?.Identity?.Name;
            var loginData = UserLoginDataRepository.GetWhere(x => x.UserName == userName).FirstOrDefault();
            if (loginData == null)
            {
                TempData.AddError("You are not logged in. You must login first.");
                return RedirectToAction("Login");
            }

            loginData.RedditRequestIdentifier = requestIdentifier;
            UserLoginDataRepository.Upsert(loginData);

            var requestURL = $"https://www.reddit.com/api/v1/authorize?client_id=x7gQpIazWKMz4A&response_type=code&state={requestIdentifier}&redirect_uri={redirectURI}&duration=temporary&scope=identity";
            return Redirect(requestURL);
        }

        [AcceptVerbs("Get", "Post")]
        public string RedditAuth(string error, string code, string state)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                string errorMsg = "";
                switch (error)
                {
                    case "access_denied":
                        errorMsg = "You denied access. You must allow access in order to link your account. Please close this window and try again.";
                        break;
                    default:
                        errorMsg = $"Error returned from Reddit service: \"{error}\". Try again, or contact site administrator if the error persists.";
                        break;
                }
                return errorMsg;
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                return "Invalid request; state code is missing. Please close this window and try again.";
            }

            var loginData = UserLoginDataRepository.GetWhere(x => x.RedditRequestIdentifier == state).FirstOrDefault();
            if (loginData == null)
            {
                return "Invalid request, or the request timed out. Please close this window and try again.";
            }

            loginData.RedditRequestIdentifier = "";
            UserLoginDataRepository.Upsert(loginData);

            var redditUsername = GetRedditUsername(code).Result;
            if (string.IsNullOrWhiteSpace(redditUsername))
            {
                return "Request could not be completed. Please close this window and try again, or contact the site administrator if the error persists.";
            }

            loginData.RedditUsername = redditUsername;
            UserLoginDataRepository.Upsert(loginData);
            return "Your Reddit account has been successfully linked. Close this window and refresh the Update Preferences page to confirm.";
        }

        //private const string redirectURI = "https%3A%2F%2Flocalhost%3A44343%2FAccount%2FRedditAuth";
        private const string redirectURI = "https%3A%2F%2Fchaossnek.com%2FAccount%2FRedditAuth";
        private async Task<string> GetRedditUsername(string code)
        {
            const string client_id = "x7gQpIazWKMz4A";
            const string client_secret = "9YTwggdrPwK6lNDInYUE0qRyQ24";
            const string authURL = "https://www.reddit.com/api/v1/access_token";
            const string apiIdentityURL = "https://oauth.reddit.com/api/v1/me";
            var post_data = $"grant_type=authorization_code&code={code}&redirect_uri={redirectURI}";
            var content = new StringContent(post_data, Encoding.UTF8, "application/x-www-form-urlencoded");

            string userName = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("chaossnek.com/1.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:{client_secret}")));

                var authResponse = await client.PostAsync(authURL, content);
                var authResponseString = await authResponse.Content.ReadAsStringAsync();

                try
                {
                    var authJsonData = JObject.Parse(authResponseString);
                    var access_token = authJsonData.SelectToken("access_token").Value<string>();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);

                    var identityResponse = await client.GetAsync(apiIdentityURL);
                    var identityResponseString = await identityResponse.Content.ReadAsStringAsync();
                    var identityJsonData = JObject.Parse(identityResponseString);
                    userName = identityJsonData.SelectToken("name").Value<string>();
                }
                catch (JsonReaderException)
                {
                    return null;
                }
            }

            return userName;
        }
    }
}
