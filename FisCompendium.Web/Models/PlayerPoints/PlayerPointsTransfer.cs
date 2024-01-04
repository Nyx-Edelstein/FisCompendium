using System.ComponentModel.DataAnnotations;

namespace FisCompendium.Web.Models.PlayerPoints
{
    public class PlayerPointsTransfer
    {
        public string FromPlayer { get; set; }

        [Required] [MinLength(1)]
        public string ToPlayer { get; set; }

        [Required] [MinLength(3)] [MaxLength(140)]
        public string Reason { get; set; }

        public int PointChange { get; set; }

        public string UpdateMessage()
        {
            if (string.IsNullOrWhiteSpace(ToPlayer) || string.IsNullOrWhiteSpace(Reason)) return "Invalid update data.";

            return $"{FromPlayer} gifted {PointChange} points to {ToPlayer}. Reason: \"{Reason}\"";
        }
    }
}
