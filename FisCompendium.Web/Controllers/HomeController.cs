using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TAC()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
