using FisCompendium.CharGen.Attributes;

namespace FisCompendium.CharGen.Enum
{
    public enum ApproachModifier
    {
        [ApproachModifier(Approaches.Forceful, 1, "Forceful +1")] Forceful,
        [ApproachModifier(Approaches.Forceful, 2, "Forceful +2")] Aggressive,
        [ApproachModifier(Approaches.Forceful, 3, "Forceful +3")] Vehement,
        [ApproachModifier(Approaches.Forceful, -1, "Forceful -1")] Passive,
        [ApproachModifier(Approaches.Forceful, -2, "Forceful -2")] Compliant,
        [ApproachModifier(Approaches.Forceful, -3, "Forceful -3")] Docile,

        [ApproachModifier(Approaches.Decisive, 1, "Decisive +1")] Decisive,
        [ApproachModifier(Approaches.Decisive, 2, "Decisive +2")] Determined,
        [ApproachModifier(Approaches.Decisive, 3, "Decisive +3")] Fateful,
        [ApproachModifier(Approaches.Decisive, -1, "Decisive -1")] Hesitant,
        [ApproachModifier(Approaches.Decisive, -2, "Decisive -2")] Reluctant,
        [ApproachModifier(Approaches.Decisive,- 3, "Decisive -3")] Timid,

        [ApproachModifier(Approaches.Flashy, 1, "Flashy +1")] Flashy,
        [ApproachModifier(Approaches.Flashy, 2, "Flashy +2")] Lively,
        [ApproachModifier(Approaches.Flashy, 3, "Flashy +3")] Exuberant,
        [ApproachModifier(Approaches.Flashy, -1, "Flashy -1")] Restrained,
        [ApproachModifier(Approaches.Flashy, -2, "Flashy -2")] Reticent,
        [ApproachModifier(Approaches.Flashy, -3, "Flashy -3")] Taciturn,

        [ApproachModifier(Approaches.Intuitive, 1, "Intuitive +1")] Intuitive,
        [ApproachModifier(Approaches.Intuitive, 2, "Intuitive +2")] Perceptive,
        [ApproachModifier(Approaches.Intuitive, 3, "Intuitive +3")] Instinctive,
        [ApproachModifier(Approaches.Intuitive, -1, "Intuitive -1")] Inattentive,
        [ApproachModifier(Approaches.Intuitive, -2, "Intuitive -2")] Careless,
        [ApproachModifier(Approaches.Intuitive, -3, "Intuitive -3")] Reckless,

        [ApproachModifier(Approaches.Methodical, 1, "Methodical +1")] Methodical,
        [ApproachModifier(Approaches.Methodical, 2, "Methodical +2")] Analytical,
        [ApproachModifier(Approaches.Methodical, 3, "Methodical +3")] Rigorous,
        [ApproachModifier(Approaches.Methodical, -1, "Methodical -1")] Disorganized,
        [ApproachModifier(Approaches.Methodical, -2, "Methodical -2")] Undisciplined,
        [ApproachModifier(Approaches.Methodical, -3, "Methodical -3")] Haphazard,

        [ApproachModifier(Approaches.Clever, 1, "Clever +1")] Clever,
        [ApproachModifier(Approaches.Clever, 2, "Clever +2")] Brilliant,
        [ApproachModifier(Approaches.Clever, 3, "Clever +3")] Genius,
        [ApproachModifier(Approaches.Clever, -1, "Clever -1")] Slow,
        [ApproachModifier(Approaches.Clever, -2, "Clever -2")] Dull,
        [ApproachModifier(Approaches.Clever, -3, "Clever -3")] Simple
    }
}
