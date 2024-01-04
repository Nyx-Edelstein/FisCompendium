using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.Account;
using FisCompendium.Web.Utilities;
using FisCompendium.Web.Utilities.UserData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class PermissionsController : Controller
    {
        public IUserPermissionLookup UserPermissionsLookup { get; }

        public PermissionsController(IUserPermissionLookup userLookup)
        {
            UserPermissionsLookup = userLookup;
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update()
        {
            var users = UserPermissionsLookup.GetUserPermissions();
            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public IActionResult Update(AccountPermission model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View();
            }

            UserPermissionsLookup.UpdatePermissionFor(model.Username, model.PermissionsLevel);
            return Update();
        }
    }
}
