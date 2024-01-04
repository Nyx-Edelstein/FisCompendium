using Microsoft.AspNetCore.Mvc.Rendering;

namespace FisCompendium.Web.Models.ThrillDigger
{
    public class ThrillDiggerModel
    {
        public string GameState { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public string StateChange { get; set; }

        public SelectListItem[] PossibleStates =
        {
            new SelectListItem("Green", "Green"),
            new SelectListItem("Blue", "Blue"),
            new SelectListItem("Red", "Red"),
            new SelectListItem("Silver", "Silver"),
            new SelectListItem("Gold", "Gold"),
            new SelectListItem("Rupoor", "Rupoor"),
        };
    }
}
