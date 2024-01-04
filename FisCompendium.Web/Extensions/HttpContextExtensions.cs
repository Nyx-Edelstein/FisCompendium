﻿using FisCompendium.Web.Models.Account;
using Microsoft.AspNetCore.Http;

namespace FisCompendium.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetCurrentUser(this HttpContext httpContext)
        {
            return GetIdentity(httpContext)?.Username;
        }

        public static string GetUserAuthTicket(this HttpContext httpContext)
        {
            return GetIdentity(httpContext)?.AuthTicket;
        }

        private static UserIdentity GetIdentity(this HttpContext httpContext)
        {
            if (!httpContext.Request.Cookies.ContainsKey("USER_IDENTITY"))
                return null;

            var serialized = httpContext.Request.Cookies["USER_IDENTITY"];
            return Newtonsoft.Json.JsonConvert.DeserializeObject<UserIdentity>(serialized);
        }
    }
}
