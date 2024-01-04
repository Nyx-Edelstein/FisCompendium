using System;
using System.Collections.Generic;
using System.Linq;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.Translator;
using FuzzyTranslator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class MiscToolsController : Controller
    {
        private static TranslatorViewModel TranslatorViewModel { get; set; }
        private static readonly object Sync = new object();

        private static List<string> PossibleLanguages => TranslatorViewModel.PossibleLanguages.Select(x => x.ToString()).ToList();

        public MiscToolsController()
        {
            lock (Sync)
            { 
                if (TranslatorViewModel == null) TranslatorViewModel = new TranslatorViewModel();
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            SetPossibleLanguages();

            return View();
        }

        [HttpPost]
        public IActionResult Index(TranslatorUpdateModel model)
        {
            SetPossibleLanguages();

            if (model == null || !PossibleLanguages.Contains(model.SelectedLanguage))
            {
                TempData.AddError("I am amused by your trickery, but it won't work.");
                return View();
            }

            lock (Sync)
            {
                TranslatorViewModel.SelectedLanguage = Enum.Parse<Language>(model.SelectedLanguage);
                TranslatorViewModel.Limit = model.Limit;
                TranslatorViewModel.InputText = model.InputText;

                var response = new TranslatorResponseModel
                {
                    FuzzyText = TranslatorViewModel.FuzzyText,
                    Cost = TranslatorViewModel.Cost
                };

                ViewData["Response"] = response;
            }

            return View();
        }

        private void SetPossibleLanguages()
        {
            ViewData["PossibleLanguages"] = PossibleLanguages;
        }
    }
}
