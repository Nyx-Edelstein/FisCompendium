using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using FisCompendium.Data.Game_Data;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Utilities.GameData.Knowledge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    public class KnowledgeController : Controller
    {
        private IKnowledgeRepository Repository { get; }

        public KnowledgeController(IKnowledgeRepository repository)
        {
            Repository = repository;
        }

        public IActionResult Error() => View();

        public IActionResult Index([FromQuery]bool showAll)
        {
            var index = Repository.GetIndex(showAll);
            return View(index);
        }

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

            data.Item.Data = ParseLinks(data.Item.Data);

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
                var item = Repository.GetById(id);

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

        [Authorize(Roles = "QM, TrustedPlayer")]
        public IActionResult NewByTitle([FromQuery] string title)
        {
            SetTags();

            return View("New", new KnowledgeItem
            {
                Title = title,
            });
        }

        [Authorize(Roles="QM, TrustedPlayer")]
        public IActionResult New(KnowledgeItem model)
        {
            SetTags();

            if (model == null)
            {
                model = new KnowledgeItem();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View(model);
            }

            model.Editor = User?.Identity?.Name ?? "Guest";

            var id = Repository.Create(model);
            if (id == Guid.Empty)
            {
                TempData.AddError("An error occurred while creating the item.");
                return View(model);
            }

            TempData.AddMessage("Item created successfully.");
            return RedirectToAction("Item", new {itemId = id.ToString()});
        }

        [Authorize(Roles = "QM, TrustedPlayer")]
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

            return View(data.Item);
        }

        [Authorize(Roles = "QM, TrustedPlayer")]
        public IActionResult SubmitEdit(KnowledgeItem model)
        {
            SetTags();

            if (!ModelState.IsValid)
            {
                TempData.AddError("Please correct validation errors.");
                return View("Edit", model);
            }

            var data = Repository.GetById(model.KnowledgeItemId);
            if (data == null)
            {
                TempData.AddError("Item not found.");
                return RedirectToAction("Index");
            }

            model.Editor = User?.Identity?.Name ?? "Guest";

            var success = Repository.Edit(model);
            if (!success)
            {
                TempData.AddError("An error occurred while editing the item.");
                return View("Edit", model);
            }

            TempData.AddMessage("Item edited successfully.");
            return RedirectToAction("Item", new { itemId = model.KnowledgeItemId.ToString() });
        }

        public IActionResult SubmitComment(KnowledgeItemComment model)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddError("Comment is too short.");
                return RedirectToAction("Item", new { itemId = model.KnowledgeItemId.ToString() });
            }

            var data = Repository.GetById(model.KnowledgeItemId);
            if (data == null)
            {
                TempData.AddError("Item not found.");
                return RedirectToAction("Index");
            }

            var username = User?.Identity?.Name ?? "Guest";
            if (username == "Guest")
            {
                TempData.AddError("You must register or login before commenting.");
                return RedirectToAction("Login", "Account");
            }

            model.Username = username;

            var success = Repository.SubmitComment(model);
            if (!success)
            {
                TempData.AddError("An error occurred while submitting the edit request.");
            }
            else
            {
                TempData.AddMessage("Comment added.");
            }

            return RedirectToAction("Item", new { itemId = model.KnowledgeItemId.ToString() });
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
