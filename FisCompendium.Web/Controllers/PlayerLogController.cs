using FisCompendium.Data.User_Data;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.PlayerPoints;
using FisCompendium.Web.Utilities.UserData.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    public class PlayerLogController : Controller
    {
        public IPlayerPointsRepository PlayerPointsUpdater { get; }

        public PlayerLogController(IPlayerPointsRepository playerPointsUpdater)
        {
            PlayerPointsUpdater = playerPointsUpdater;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            SetData();
            return View();
        }

        [HttpPost]
        public IActionResult Transfer(PlayerPointsTransfer model)
        {
            model.FromPlayer = HttpContext.GetCurrentUser();
            SetData();

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View("Index");
            }

            if (model.PointChange <= 0)
            {
                TempData.AddError("Transferred points must be positive.");
                return View("Index");
            }

            var currentPlayerPoints = PlayerPointsUpdater.GetCurrentPointsForPlayer(model.FromPlayer);
            if (currentPlayerPoints < model.PointChange)
            {
                TempData.AddError($"You do not have enough points. Your current total is {currentPlayerPoints}.");
                return View("Index");
            }

            var validUsernames =  PlayerPointsUpdater.GetValidPlayerNames();
            if (!validUsernames.Contains(model.ToPlayer))
            {
                TempData.AddError("The user you have specified is not valid.");
                return View("Index");
            }

            PlayerPointsUpdater.CompleteTransfer(model);
            TempData.AddMessage($"{model.PointChange} points have been transferred to {model.ToPlayer} and will be released following the next update.");
            return View("Index");
        }

        private void SetData()
        {
            ViewData["ValidPlayerNames"] = PlayerPointsUpdater.GetValidPlayerNames();
            ViewData["PlayerLog"] = PlayerPointsUpdater.GetPlayerLogs(50);

            var currentUser = HttpContext.GetCurrentUser();
            ViewData["TopPlayersCurrent"] = PlayerPointsUpdater.GetTopCurrentPlayers(50);
            ViewData["UserPoints"] = !string.IsNullOrWhiteSpace(currentUser) ? PlayerPointsUpdater.GetUserPoints(currentUser) : null;
        }
    }
}
