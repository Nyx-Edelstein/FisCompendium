using System.Collections.Generic;
using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Interfaces;
using FisCompendium.CharGen.Models.Variables;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.RacialVariables
{
    public class BlinVariables : IRacialVariablesSelector
    {
        public VariableCollection Select()
        {
            var collection = new VariableCollection();

            collection.Var_Phys_Recovery = new IntegerStatVariable(3, 4, 6);
            collection.Var_Soc_Recovery = new IntegerStatVariable(3, 5, 6);
            collection.Var_Mag_Recovery = new IntegerStatVariable(5, 8, 10);

            collection.Var_Phys_MaxStress = new IntegerStatVariable(18, 25, 34);
            collection.Var_Soc_MaxStress = new IntegerStatVariable(12, 16, 22);
            collection.Var_Mag_MaxStress = new IntegerStatVariable(12, 16, 22);

            collection.Var_FateCap = new IntegerStatVariable(4, 5, 6);
            collection.Var_Luck = new RealStatVariable(0.15, 0.35, 1.0);

            collection.RedAffinityRate = 0.1;
            collection.GreenAffinityRate = 0.05;
            collection.BlueAffinityRate = 0.05;
            collection.PrismaticAffinityRate = 0.0;
            collection.VoidAffinityRate = 0.0;

            collection.Var_RedAffinity = new RealStatVariable(0.3, 1.25, 5.0);
            collection.Var_GreenAffinity = new RealStatVariable(0.1, 0.66, 3.0);
            collection.Var_BlueAffinity = new RealStatVariable(0.1, 0.66, 3.0);
            collection.Var_PrismaticAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_VoidAffinity = new RealStatVariable(0, 0, 0);

            var categoryModifiers = new List<WeightedItem<CategoryModifier>>
            {
                WeightedItem.Create(CategoryModifier.Child_Of_Din, 50.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Din, 10.0),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Din, 2.0),

                WeightedItem.Create(CategoryModifier.Child_Of_Farore, 25.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Farore, 5.0),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Farore, 1.0),

                WeightedItem.Create(CategoryModifier.Child_Of_Nayru, 25.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Nayru, 5.0),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Nayru, 1.0),

                WeightedItem.Create(CategoryModifier.Hearty, 45.0),
                WeightedItem.Create(CategoryModifier.Imposing, 25.0),
                WeightedItem.Create(CategoryModifier.Commanding, 15.0),

                WeightedItem.Create(CategoryModifier.Modest, 2.5),
                WeightedItem.Create(CategoryModifier.Frail, 0.5),
                WeightedItem.Create(CategoryModifier.Feeble, 0.1),

                WeightedItem.Create(CategoryModifier.Charming, 2.50),
                WeightedItem.Create(CategoryModifier.Charismatic, 0.5),
                WeightedItem.Create(CategoryModifier.Enthralling, 0.1),

                WeightedItem.Create(CategoryModifier.Disagreeable, 25.0),
                WeightedItem.Create(CategoryModifier.Obnoxious, 5.0),
                WeightedItem.Create(CategoryModifier.Repulsive, 1.0),

                WeightedItem.Create(CategoryModifier.Adept, 5.0),
                WeightedItem.Create(CategoryModifier.Erudite, 1.0),
                WeightedItem.Create(CategoryModifier.Savant, 0.2),

                WeightedItem.Create(CategoryModifier.Unwieldy, 25.0),
                WeightedItem.Create(CategoryModifier.Inelegant, 5.0),
                WeightedItem.Create(CategoryModifier.Inept, 1.0),
            };

            collection.Var_CategoryModDistribution = new DistributionVariable<CategoryModifier>(categoryModifiers);

            var approachModifiers = new List<WeightedItem<ApproachModifier>>
            {
                WeightedItem.Create(ApproachModifier.Forceful, 100.0),
                WeightedItem.Create(ApproachModifier.Aggressive, 75.0),
                WeightedItem.Create(ApproachModifier.Vehement, 15.0),

                WeightedItem.Create(ApproachModifier.Passive, 10.0),
                WeightedItem.Create(ApproachModifier.Compliant, 5.0),
                WeightedItem.Create(ApproachModifier.Docile, 1.0),

                WeightedItem.Create(ApproachModifier.Decisive, 100.0),
                WeightedItem.Create(ApproachModifier.Determined, 75.0),
                WeightedItem.Create(ApproachModifier.Fateful, 15.0),

                WeightedItem.Create(ApproachModifier.Hesitant, 10.0),
                WeightedItem.Create(ApproachModifier.Reluctant, 5.0),
                WeightedItem.Create(ApproachModifier.Timid, 1.0),

                WeightedItem.Create(ApproachModifier.Flashy, 10.0),
                WeightedItem.Create(ApproachModifier.Lively, 5.0),
                WeightedItem.Create(ApproachModifier.Exuberant, 1.0),

                WeightedItem.Create(ApproachModifier.Restrained, 10.0),
                WeightedItem.Create(ApproachModifier.Reticent, 5.0),
                WeightedItem.Create(ApproachModifier.Taciturn, 1.0),

                WeightedItem.Create(ApproachModifier.Intuitive, 10.0),
                WeightedItem.Create(ApproachModifier.Perceptive, 5.0),
                WeightedItem.Create(ApproachModifier.Instinctive, 1.0),

                WeightedItem.Create(ApproachModifier.Inattentive, 10.0),
                WeightedItem.Create(ApproachModifier.Careless, 5.0),
                WeightedItem.Create(ApproachModifier.Reckless, 1.0),

                WeightedItem.Create(ApproachModifier.Methodical, 10.0),
                WeightedItem.Create(ApproachModifier.Analytical, 5.0),
                WeightedItem.Create(ApproachModifier.Rigorous, 1.0),

                WeightedItem.Create(ApproachModifier.Disorganized, 10.0),
                WeightedItem.Create(ApproachModifier.Undisciplined, 5.0),
                WeightedItem.Create(ApproachModifier.Haphazard, 1.0),

                WeightedItem.Create(ApproachModifier.Clever, 0.50),
                WeightedItem.Create(ApproachModifier.Brilliant, 0.25),
                WeightedItem.Create(ApproachModifier.Genius, 0.01),

                WeightedItem.Create(ApproachModifier.Slow, 100.0),
                WeightedItem.Create(ApproachModifier.Dull, 50.0),
                WeightedItem.Create(ApproachModifier.Simple, 10.0),
            };

            collection.Var_ApproachModDistribution = new DistributionVariable<ApproachModifier>(approachModifiers);

            return collection;
        }
    }
}
