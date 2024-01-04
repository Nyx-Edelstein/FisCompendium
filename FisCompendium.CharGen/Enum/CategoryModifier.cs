using FisCompendium.CharGen.Attributes;

namespace FisCompendium.CharGen.Enum
{
    public enum CategoryModifier
    {
        [CategoryModifier(Category.Red, 1, "Red +1")] Child_Of_Din,
        [CategoryModifier(Category.Red, 2, "Red +2")] Favored_Of_Din,
        [CategoryModifier(Category.Red, 3, "Red +3")] Blessed_Of_Din,

        [CategoryModifier(Category.Green, 1, "Green +1")] Child_Of_Farore,
        [CategoryModifier(Category.Green, 2, "Green +2")] Favored_Of_Farore,
        [CategoryModifier(Category.Green, 3, "Green +3")] Blessed_Of_Farore,

        [CategoryModifier(Category.Blue, 1, "Blue +1")] Child_Of_Nayru,
        [CategoryModifier(Category.Blue, 2, "Blue +2")] Favored_Of_Nayru,
        [CategoryModifier(Category.Blue, 3, "Blue +3")] Blessed_Of_Nayru,

        [CategoryModifier(Category.Physical, 1, "Physical +1")] Hearty,
        [CategoryModifier(Category.Physical, 2, "Physical +2")] Imposing,
        [CategoryModifier(Category.Physical, 3, "Physical +3")] Commanding,
        [CategoryModifier(Category.Physical, -1, "Physical -1")] Modest,
        [CategoryModifier(Category.Physical, -2, "Physical -2")] Frail,
        [CategoryModifier(Category.Physical, -3, "Physical -3")] Feeble,

        [CategoryModifier(Category.Social, 1, "Social +1")] Charming,
        [CategoryModifier(Category.Social, 2, "Social +2")] Charismatic,
        [CategoryModifier(Category.Social, 3, "Social +3")] Enthralling,
        [CategoryModifier(Category.Social, -1, "Social -1")] Disagreeable,
        [CategoryModifier(Category.Social, -2, "Social -2")] Obnoxious,
        [CategoryModifier(Category.Social, -3, "Social -3")] Repulsive,

        [CategoryModifier(Category.Magical, 1, "Magical +1")] Adept,
        [CategoryModifier(Category.Magical, 2, "Magical +2")] Erudite,
        [CategoryModifier(Category.Magical, 3, "Magical +3")] Savant,
        [CategoryModifier(Category.Magical, -1, "Magical -1")] Unwieldy,
        [CategoryModifier(Category.Magical, -2, "Magical -2")] Inelegant,
        [CategoryModifier(Category.Magical, -3, "Magical -3")] Inept
    }
}
