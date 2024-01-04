using System;
using FisCompendium.CharGen.Enum;

namespace FisCompendium.CharGen.Attributes
{
    public class CategoryModifierAttribute : Attribute
    {
        public Category Category { get; }
        public int Shifts { get; }
        public string Description { get; }

        public CategoryModifierAttribute(Category category, int shifts, string description)
        {
            Category = category;
            Shifts = shifts;
            Description = description;
        }
    }
}
