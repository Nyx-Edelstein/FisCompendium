using System;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.ThrillDigger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThrillDigger.Enum;
using ThrillDigger.Models;

namespace FisCompendium.Web.Controllers
{
    public class ThrillDiggerController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            var gameGrid = new GameGrid();
            ViewData["GameGrid"] = gameGrid;

            var model = new ThrillDiggerModel
            {
                GameState = JsonConvert.SerializeObject(gameGrid.Grid)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ThrillDiggerModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GameState))
            {
                TempData.AddError("Don't screw with the code or it won't work.");
                SetDefaultGrid();
                return View(model);
            }

            if (model.Col < 0 || model.Col > 7)
            {
                TempData.AddError("Column value out of bounds.");
                SetDefaultGrid();
                return View(model);
            }

            if (model.Row < 0 || model.Row > 4)
            {
                TempData.AddError("Row value out of bounds.");
                SetDefaultGrid();
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.StateChange))
            {
                TempData.AddError("Invalid state change.");
                SetDefaultGrid();
                return View(model);
            }

            NodeState stateChange;
            var parsed = Enum.TryParse<NodeState>(model.StateChange, true, out stateChange);
            if (!parsed)
            {
                TempData.AddError("Invalid state change.");
                SetDefaultGrid();
                return View(model);
            }

            try
            {
                var gameGrid = new GameGrid(model.GameState);
                gameGrid.Update(model.Row, model.Col, stateChange);

                ViewData["GameGrid"] = gameGrid;

                var newModel = new ThrillDiggerModel
                {
                    GameState = JsonConvert.SerializeObject(gameGrid.Grid)
                };
                return View(newModel);
            }
            catch (Exception e)
            {
                TempData.AddError("Something went wrong...");
                SetDefaultGrid();
                return View(model);
            }
        }

        private void SetDefaultGrid()
        {
            var gameGrid = new GameGrid();
            ViewData["GameGrid"] = gameGrid;
        }
    }
}
