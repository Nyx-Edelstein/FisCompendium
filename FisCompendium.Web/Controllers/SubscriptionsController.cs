using System.Linq;
using FisCompendium.Data.User_Data;
using FisCompendium.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class SubscriptionsController : Controller
    {
        private IRepository<UserLoginData> UserLoginDataRepository { get; }

        public SubscriptionsController(IRepository<UserLoginData> _userLoginDataRepository)
        {
            UserLoginDataRepository = _userLoginDataRepository;
        }

        public IActionResult Error()
        {
            return View();
        }

        [Authorize("IsQM")]
        public IActionResult Index()
        {
            var subscribedUsers = UserLoginDataRepository.GetWhere(x => x.Subscribed)
                .ToList()
                .Select(x => x.RecoveryEmail)
                .ToArray();

            ViewData["Emails"] = subscribedUsers;

            return View();
        }
    }
}
