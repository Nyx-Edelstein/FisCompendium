using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class CombatController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        [Authorize("IsQM")]
        public IActionResult Grid()
        {
            return View();
        }

        [Authorize("IsQM")]
        public IActionResult Tools()
        {
            return View();
        }
    }
}
