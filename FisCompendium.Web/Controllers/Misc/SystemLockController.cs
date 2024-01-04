using System;
using FisCompendium.Web.Utilities.SystemData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class SystemLockController : Controller
    {
        public ISystemLockRepository Repository { get; }

        public SystemLockController(ISystemLockRepository repository)
        {
            Repository = repository;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            var Username = User?.Identity?.Name ?? "Guest";
            if (!string.Equals(Username, @"Vecht", StringComparison.InvariantCultureIgnoreCase)) return RedirectToAction("Error");

            ViewData["SystemLock"] = Repository.GetIsSystemLocked();
            return View();
        }

        [HttpPost]
        public IActionResult Lock()
        {
            var Username = User?.Identity?.Name ?? "Guest";
            if (!string.Equals(Username, @"Vecht", StringComparison.InvariantCultureIgnoreCase)) return RedirectToAction("Error");

            Repository.LockSystem();

            return RedirectToAction("Index");
        }
    }
}
