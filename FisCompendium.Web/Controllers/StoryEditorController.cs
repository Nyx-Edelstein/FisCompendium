using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class StoryEditorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        [Authorize("IsQM")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
