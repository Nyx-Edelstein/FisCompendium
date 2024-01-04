using System;
using FisCompendium.CharGen.Enum;

namespace FisCompendium.CharGen.Attributes
{
    public class ApproachModifierAttribute : Attribute
    {
        public Approaches Approach { get; }
        public int Shifts { get; }
        public string Description { get; }

        public ApproachModifierAttribute(Approaches approach, int shifts, string description)
        {
            Approach = approach;
            Shifts = shifts;
            Description = description;
        }
    }
}
