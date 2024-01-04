using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FisCompendium.Web.Models.PlayerPoints
{
    public class PlayerPointsUpdate
    {
        [Required] [MinLength(1)]
        public string PlayerNames { get; set; } 

        [Required]
        public string Reason { get; set; }

        public int PointChange { get; set; }

        public string UpdateMessage()
        {
            if (PlayerNames == null || !PlayerNames.Any() || string.IsNullOrWhiteSpace(Reason)) return "Invalid update data.";

            var changeString = PointChange < 0 ? "Removed" : "Added";
            var updateString = PointChange < 0 ? "from" : "to";
            return $"{changeString} {Math.Abs(PointChange)} points for {Reason} {updateString} users: {string.Join(", ", UsernameList)}";
        }

        public List<string> UsernameList => PlayerNames?.Split(',').ToList() ?? new List<string>();
    }
}
