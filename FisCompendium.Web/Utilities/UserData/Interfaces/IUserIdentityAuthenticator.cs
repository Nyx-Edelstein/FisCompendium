using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FisCompendium.Web.Utilities.UserData.Interfaces
{
    public interface IUserIdentityAuthenticator
    {
        Task GetUserIdentity(HttpContext httpContext, Func<Task> next);
    }
}
