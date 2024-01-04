using System.Collections.Generic;
using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Interfaces;
using FisCompendium.CharGen.Models.Variables;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.RacialVariables
{
    public class LizalfosVariables : IRacialVariablesSelector
    {
        public VariableCollection Select()
        {
            var collection = new VariableCollection();

            collection.Var_Phys_Recovery = new IntegerStatVariable(1, 3, 4);
            collection.Var_Soc_Recovery = new IntegerStatVariable(2, 3, 4);
            collection.Var_Mag_Recovery = new IntegerStatVariable(4, 6, 9);

            collection.Var_Phys_MaxStress = new IntegerStatVariable(8, 18, 20);
            collection.Var_Soc_MaxStress = new IntegerStatVariable(8, 18, 20);
            collection.Var_Mag_MaxStress = new IntegerStatVariable(8, 18, 20);

            collection.Var_FateCap = new IntegerStatVariable(4, 5, 6);
            collection.Var_Luck = new RealStatVariable(0.5, 1.0, 1.5);

            collection.RedAffinityRate = 0.075;
            collection.GreenAffinityRate = 0.25;
            collection.BlueAffinityRate = 0.075;
            collection.PrismaticAffinityRate = 0.0;
            collection.VoidAffinityRate = 0.0;

            collection.Var_RedAffinity = new RealStatVariable(0.15, 0.75, 4.0);
            collection.Var_GreenAffinity = new RealStatVariable(0.3, 1.25, 6.0);
            collection.Var_BlueAffinity = new RealStatVariable(0.15, 0.75, 4.0);
            collection.Var_PrismaticAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_VoidAffinity = new RealStatVariable(0, 0, 0);

            var categoryModifiers = new List<WeightedItem<CategoryModifier>>
            {
                WeightedItem.Create(CategoryModifier.Child_Of_Din, 15.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Din, 2.5),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Din, 0.5),

                WeightedItem.Create(CategoryModifier.Child_Of_Farore, 50.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Farore, 10.0),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Farore, 2.0),

                WeightedItem.Create(CategoryModifier.Child_Of_Nayru, 15.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Nayru, 2.5),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Nayru, 0.5),

                WeightedItem.Create(CategoryModifier.Hearty, 10.0),
                WeightedItem.Create(CategoryModifier.Imposing, 3.0),
                WeightedItem.Create(CategoryModifier.Commanding, 0.6),
                WeightedItem.Create(CategoryModifier.Modest, 50.0),
                WeightedItem.Create(CategoryModifier.Frail, 10.0),
                WeightedItem.Create(CategoryModifier.Feeble, 2.0),

                WeightedItem.Create(CategoryModifier.Charming, 25.0),
                WeightedItem.Create(CategoryModifier.Charismatic, 5.0),
                WeightedItem.Create(CategoryModifier.Enthralling, 1.0),
                WeightedItem.Create(CategoryModifier.Disagreeable, 25.0),
                WeightedItem.Create(CategoryModifier.Obnoxious, 5.0),
                WeightedItem.Create(CategoryModifier.Repulsive, 1.0),

                WeightedItem.Create(CategoryModifier.Adept, 125.0),
                WeightedItem.Create(CategoryModifier.Erudite, 25.0),
                WeightedItem.Create(CategoryModifier.Savant, 5.0),
                WeightedItem.Create(CategoryModifier.Unwieldy, 10.0),
                WeightedItem.Create(CategoryModifier.Inelegant, 2.0),
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
                WeightedItem.Create(ApproachModifier.Flashy, 100.0),
                WeightedItem.Create(ApproachModifier.Lively, 50.0),
                WeightedItem.Create(ApproachModifier.Exuberant, 10.0),
                WeightedItem.Create(ApproachModifier.Restrained, 1.0),
                WeightedItem.Create(ApproachModifier.Reticent, 0.5),
                WeightedItem.Create(ApproachModifier.Taciturn, 0.1),
                WeightedItem.Create(ApproachModifier.Intuitive, 100.0),
                WeightedItem.Create(ApproachModifier.Perceptive, 50.0),
                WeightedItem.Create(ApproachModifier.Instinctive, 10.0),
                WeightedItem.Create(ApproachModifier.Inattentive, 1.0),
                WeightedItem.Create(ApproachModifier.Careless, 0.5),
                WeightedItem.Create(ApproachModifier.Reckless, 0.1),
                WeightedItem.Create(ApproachModifier.Methodical, 10.0),
                WeightedItem.Create(ApproachModifier.Analytical, 5.0),
                WeightedItem.Create(ApproachModifier.Rigorous, 1.0),
                WeightedItem.Create(ApproachModifier.Disorganized, 10.0),
                WeightedItem.Create(ApproachModifier.Undisciplined, 5.0),
                WeightedItem.Create(ApproachModifier.Haphazard, 1.0),
                WeightedItem.Create(ApproachModifier.Clever, 10.0),
                WeightedItem.Create(ApproachModifier.Brilliant, 5.0),
                WeightedItem.Create(ApproachModifier.Genius, 1.0),
                WeightedItem.Create(ApproachModifier.Slow, 10.0),
                WeightedItem.Create(ApproachModifier.Dull, 5.0),
                WeightedItem.Create(ApproachModifier.Simple, 1.0),
            };

            collection.Var_ApproachModDistribution = new DistributionVariable<ApproachModifier>(approachModifiers);

            return collection;
        }
    }
}
