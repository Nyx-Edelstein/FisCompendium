using System.Linq;
using FisCompendium.Data.System_Data;
using FisCompendium.Repository;
using FisCompendium.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    [Authorize("IsQM")]
    public class ConfigurationController : Controller
    {
        public IRepository<ConfigItem> ConfigItemRepository { get; }

        public ConfigurationController(IRepository<ConfigItem> configItemRepository)
        {
            ConfigItemRepository = configItemRepository;
        }

        public IActionResult Error()
        {
            return View();
        }

        [Authorize("IsQM")]
        public IActionResult Index()
        {
            var configItems = ConfigItemRepository.GetWhere(x => true)
                .OrderBy(x => x.Key)
                .ToList();
            return View(configItems);
        }

        [Authorize("IsQM")]
        public IActionResult UpdateConfigItem(ConfigItem model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Key))
            {
                TempData.AddError("No key specified.");
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(model.Value))
            {
                TempData.AddError("Value not specified.");
                return RedirectToAction("Index");
            }

            var configItem = ConfigItemRepository.GetWhere(x => x.Key == model.Key)
                .FirstOrDefault();

            bool success;
            if (configItem == null)
            {
                var newItem = new ConfigItem
                {
                    Key = model.Key,
                    Value = model.Value
                };

                success = ConfigItemRepository.Upsert(newItem);
            }
            else
            {
                configItem.Value = model.Value;
                success = ConfigItemRepository.Upsert(configItem);
            }

            if (success)
            {
                TempData.AddMessage("Config item updated successfully.");
            }
            else
            {
                TempData.AddError("Something went wrong.");
            }

            return RedirectToAction("Index");
        }
    }
}
