using System.Collections.Generic;
using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Interfaces;
using FisCompendium.CharGen.Models.Variables;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.RacialVariables
{
    public class ZoraVariables : IRacialVariablesSelector
    {
        public VariableCollection Select()
        {
            var collection = new VariableCollection();

            collection.Var_Phys_Recovery = new IntegerStatVariable(5, 6, 8);
            collection.Var_Soc_Recovery = new IntegerStatVariable(1, 1, 4);
            collection.Var_Mag_Recovery = new IntegerStatVariable(1, 1, 4);

            collection.Var_Phys_MaxStress = new IntegerStatVariable(10, 14, 16);
            collection.Var_Soc_MaxStress = new IntegerStatVariable(20, 35, 50);
            collection.Var_Mag_MaxStress = new IntegerStatVariable(24, 26, 30);

            collection.Var_FateCap = new IntegerStatVariable(8, 10, 12);
            collection.Var_Luck = new RealStatVariable(0.25, 0.5, 1.0);

            collection.RedAffinityRate = 0.025;
            collection.GreenAffinityRate = 0.025;
            collection.BlueAffinityRate = 0.25;
            collection.PrismaticAffinityRate = 0.0;
            collection.VoidAffinityRate = 0.0;

            collection.Var_RedAffinity = new RealStatVariable(0.1, 0.33, 1.25);
            collection.Var_GreenAffinity = new RealStatVariable(0.1, 0.33, 1.25);
            collection.Var_BlueAffinity = new RealStatVariable(0.4, 2.0, 10.0);
            collection.Var_PrismaticAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_VoidAffinity = new RealStatVariable(0, 0, 0);

            var categoryModifiers = new List<WeightedItem<CategoryModifier>>
            {
                WeightedItem.Create(CategoryModifier.Child_Of_Din, 1.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Din, 0.2),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Din, 0.04),

                WeightedItem.Create(CategoryModifier.Child_Of_Farore, 1.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Farore, 0.2),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Farore, 0.04),

                WeightedItem.Create(CategoryModifier.Child_Of_Nayru, 35.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Nayru, 15.0),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Nayru, 5.0),

                WeightedItem.Create(CategoryModifier.Hearty, 5.0),
                WeightedItem.Create(CategoryModifier.Imposing, 1.0),
                WeightedItem.Create(CategoryModifier.Commanding, 0.2),
                WeightedItem.Create(CategoryModifier.Modest, 50.0),
                WeightedItem.Create(CategoryModifier.Frail, 10.0),
                WeightedItem.Create(CategoryModifier.Feeble, 2.0),

                WeightedItem.Create(CategoryModifier.Charming, 125.0),
                WeightedItem.Create(CategoryModifier.Charismatic, 50.0),
                WeightedItem.Create(CategoryModifier.Enthralling, 15.0),
                WeightedItem.Create(CategoryModifier.Disagreeable, 10.0),
                WeightedItem.Create(CategoryModifier.Obnoxious, 2.5),
                WeightedItem.Create(CategoryModifier.Repulsive, 0.5),

                WeightedItem.Create(CategoryModifier.Adept, 55.0),
                WeightedItem.Create(CategoryModifier.Erudite, 15.0),
                WeightedItem.Create(CategoryModifier.Savant, 3.0),
                WeightedItem.Create(CategoryModifier.Unwieldy, 15.0),
                WeightedItem.Create(CategoryModifier.Inelegant, 3.0),
                WeightedItem.Create(CategoryModifier.Inept, 0.5),
            };

            collection.Var_CategoryModDistribution = new DistributionVariable<CategoryModifier>(categoryModifiers);

            var approachModifiers = new List<WeightedItem<ApproachModifier>>
            {
                WeightedItem.Create(ApproachModifier.Forceful, 10.0),
                WeightedItem.Create(ApproachModifier.Aggressive, 5.0),
                WeightedItem.Create(ApproachModifier.Vehement, 1.0),
                WeightedItem.Create(ApproachModifier.Passive, 10.0),
                WeightedItem.Create(ApproachModifier.Compliant, 5.0),
                WeightedItem.Create(ApproachModifier.Docile, 1.0),

                WeightedItem.Create(ApproachModifier.Decisive, 10.0),
                WeightedItem.Create(ApproachModifier.Determined, 5.0),
                WeightedItem.Create(ApproachModifier.Fateful, 1.0),
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

                WeightedItem.Create(ApproachModifier.Methodical, 50.0),
                WeightedItem.Create(ApproachModifier.Analytical, 25.0),
                WeightedItem.Create(ApproachModifier.Rigorous, 5.0),
                WeightedItem.Create(ApproachModifier.Disorganized, 2.0),
                WeightedItem.Create(ApproachModifier.Undisciplined, 1.0),
                WeightedItem.Create(ApproachModifier.Haphazard, 0.2),

                WeightedItem.Create(ApproachModifier.Clever, 50.0),
                WeightedItem.Create(ApproachModifier.Brilliant, 25.0),
                WeightedItem.Create(ApproachModifier.Genius, 5.0),
                WeightedItem.Create(ApproachModifier.Slow, 2.0),
                WeightedItem.Create(ApproachModifier.Dull, 1.0),
                WeightedItem.Create(ApproachModifier.Simple, 0.2),
            };

            collection.Var_ApproachModDistribution = new DistributionVariable<ApproachModifier>(approachModifiers);

            return collection;
        }
    }
}
