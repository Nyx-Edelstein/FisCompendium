using System.Collections.Generic;

namespace FisCompendium.Web.Models.Status
{
    public class StatusData
    {
        public int CurrentVoidPower { get; set; }
        public int CurrentStoredMagic { get; set; }
        public CharacterStatus [] Characters { get; set; }
    }
}
