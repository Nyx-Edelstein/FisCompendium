using FisCompendium.CharGen.Attributes;

namespace FisCompendium.CharGen.Enum
{
    public enum Race
    {
        [RaceXPBreakpoints( 10,  16,  32, 0.75)] Blin,
        [RaceXPBreakpoints(  0,   1,  12, 0.70)] Deku,
        [RaceXPBreakpoints( 11,  19,  46, 1.00)] Gerudo,
        [RaceXPBreakpoints(  3,  33, 150, 0.20)] Goron,
        [RaceXPBreakpoints( 12,  22,  42, 1.00)] Hylian,
        [RaceXPBreakpoints(  4,  10,  28, 0.85)] Lizalfos,
        [RaceXPBreakpoints(  2,   6,  11, 0.60)] Lynel,
        [RaceXPBreakpoints(  8,  40, 130, 0.10)] Zora,
        Entity,
        Other_Being,
        Creature
    }
}
