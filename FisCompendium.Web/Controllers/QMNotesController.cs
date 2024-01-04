using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using FisCompendium.Data.Game_Data;
using FisCompendium.Data.User_Data;
using FisCompendium.Repository;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Utilities.GameData.Knowledge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class QMNotesController : Controller
    {
        private IQMNoteRepository Repository { get; }

        public QMNotesController(IQMNoteRepository repository)
        {
            Repository = repository;
        }

        [Authorize("IsQM")]
        public IActionResult Error() => View();

        [Authorize("IsQM")]
        public IActionResult Index([FromQuery]bool showAll)
        {
            var index = Repository.GetIndex(showAll);
            return View(index);
        }

        [Authorize("IsQM")]
        public IActionResult Item([FromQuery] string itemId, [FromQuery] string title = "")
        {
            if (!Guid.TryParse(itemId, out var id))
            {
                TempData.AddError("Id could not be parsed.");
                return RedirectToAction("Index");
            }

            var data = Repository.GetById(id);
            if (data == null)
            {
                TempData.AddError("Item not found.");
                return RedirectToAction("Index");
            }

            data.Notes = ParseLinks(data.Notes);

            return View("Item", data);
        }

        private IEnumerable<string> GetSubStrings(string input, string start, string end)
        {
            Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
            MatchCollection matches = r.Matches(input);
            foreach (Match match in matches)
                yield return match.Groups[1].Value;
        }

        private string ParseLinks(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var replacements = GetSubStrings(input, "[[", "]]")
                .Select(x => $"[[{x}]]")
                .ToList();
            if (!replacements.Any()) return input;

            string output = input + "";
            foreach (var replacement in replacements)
            {
                var title = replacement.Replace("[[", "").Replace("]]", "");
                var id = Repository.GetItemId(title);

                if (id == Guid.Empty)
                {
                    var url = Url.Action("NewByTitle");
                    var html = $"<a href='{url}?title={WebUtility.UrlEncode(title)}' style='color: red' target='_blank'>{title}</a>";
                    output = output.Replace(replacement, html);
                }
                else
                {
                    var url = Url.Action("Item");
                    var html = $"<a href='{url}?itemId={id}'>{title}</a>";
                    output = output.Replace(replacement, html);
                }
            }

            return output;
        }

        [Authorize("IsQM")]
        public IActionResult NewByTitle([FromQuery] string title)
        {
            SetTags();

            return View("New", new QMNote
            {
                Title = title,
            });
        }

        [Authorize("IsQM")]
        public IActionResult New(QMNote model)
        {
            SetTags();

            if (model == null)
            {
                model = new QMNote();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View(model);
            }

            var id = Repository.Create(model);
            if (id == Guid.Empty)
            {
                TempData.AddError("An error occurred while creating the item.");
                return View(model);
            }

            TempData.AddMessage("Item created successfully.");
            return RedirectToAction("Item", new {itemId = id.ToString()});
        }

        [Authorize("IsQM")]
        public IActionResult Edit([FromQuery] string itemId)
        {
            SetTags();

            if (!Guid.TryParse(itemId, out var id))
            {
                TempData.AddError("Id could not be parsed.");
                return RedirectToAction("Index");
            }

            var data = Repository.GetById(id);
            if (data == null)
            {
                TempData.AddError("Item not found.");
                return RedirectToAction("Index");
            }

            return View(data);
        }

        [Authorize("IsQM")]
        public IActionResult SubmitEdit(QMNote model)
        {
            SetTags();

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View("Edit", model);
            }

            var data = Repository.GetById(model.QMNoteId);
            if (data == null)
            {
                TempData.AddError("Item not found.");
                return RedirectToAction("Index");
            }


            var success = Repository.Edit(model);
            if (!success)
            {
                TempData.AddError("An error occurred while editing the item.");
                return View("Edit", model);
            }

            TempData.AddMessage("Item edited successfully.");
            return RedirectToAction("Item", new { itemId = model.QMNoteId.ToString() });
        }

        private void SetTags()
        {
            var tags = Repository.GetIndex(includeHidden: false)
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewData["ExistingTags"] = tags;
        }
    }
}
