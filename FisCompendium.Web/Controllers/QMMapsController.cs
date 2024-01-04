using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class QMMapsController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        [Authorize("IsQM")]
        public IActionResult World()
        {
            return View();
        }
    }
}
