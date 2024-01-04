using System.Collections.Generic;

namespace FisCompendium.Web.Models.Status
{
    public class CharacterStatus
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public int MagicPercent { get; set; }
        public int MagicDamagePercent { get; set; }

        public int HeartsFull { get; set; }
        public HeartState HeartsPartial { get; set; }
        public int HeartsEmpty { get; set; }

        public int StaminaFullWheels { get; set; }
        public int StaminaCurrentRemaining { get; set; }
        public int StaminaCurrentUsed { get; set; }
        public int StaminaCurrentMissing => 20 - (StaminaCurrentRemaining + StaminaCurrentUsed);
        public int StaminaEmptyWheels { get; set; }

        public int FatiguePercent { get; set; }
    }
}
