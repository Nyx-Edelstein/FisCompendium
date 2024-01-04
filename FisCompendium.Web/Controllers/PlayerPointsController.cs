using System.Linq;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.PlayerPoints;
using FisCompendium.Web.Utilities.UserData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class PlayerPointsController : Controller
    {
        public IPlayerPointsRepository PlayerPointsUpdater { get; }

        public PlayerPointsController(IPlayerPointsRepository playerPointsUpdater)
        {
            PlayerPointsUpdater = playerPointsUpdater;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Update()
        {
            SetData();
            return View();
        }

        [HttpPost]
        public IActionResult Update(PlayerPointsUpdate model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Correct validation errors.");
                return View();
            }

            var validationResult = ValidateUsernames(model.PlayerNames);
            if (validationResult != "true")
            {
                TempData.AddError(validationResult);
                return View();
            }

            PlayerPointsUpdater.Update(model);

            TempData.AddMessage(model.UpdateMessage());
            SetData();
            return View();
        }

        [HttpPost]
        public IActionResult ReleasePendingPoints()
        {
            PlayerPointsUpdater.ReleasePendingPoints();

            TempData.AddMessage("Pending points for all players have been released to their current points pool.");
            SetData();
            return View("Update");
        }

        private void SetData()
        {
            ViewData["ValidPlayerNames"] = PlayerPointsUpdater.GetValidPlayerNames();
            ViewData["PlayerLog"] = PlayerPointsUpdater.GetPlayerLogs(50);
        }

        [AcceptVerbs("Get", "Post")]
        public string ValidateUsernames(string usernames)
        {
            if (string.IsNullOrWhiteSpace(usernames)) return "At least one username must be supplied";

            var usernameList = usernames.Split(',').ToList();

            return !usernameList.Any() ? "At least one username must be supplied" : "true";
        }
    }
}
