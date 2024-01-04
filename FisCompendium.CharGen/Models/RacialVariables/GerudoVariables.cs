using System.Collections.Generic;
using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Interfaces;
using FisCompendium.CharGen.Models.Variables;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.RacialVariables
{
    public class GerudoVariables : IRacialVariablesSelector
    {
        public VariableCollection Select()
        {
            var collection = new VariableCollection();

            collection.Var_Phys_Recovery = new IntegerStatVariable(4, 5, 8);
            collection.Var_Soc_Recovery = new IntegerStatVariable(2, 4, 5);
            collection.Var_Mag_Recovery = new IntegerStatVariable(4, 6, 9);

            collection.Var_Phys_MaxStress = new IntegerStatVariable(20, 22, 24);
            collection.Var_Soc_MaxStress = new IntegerStatVariable(16, 18, 20);
            collection.Var_Mag_MaxStress = new IntegerStatVariable(18, 20, 22);

            collection.Var_FateCap = new IntegerStatVariable(8, 10, 12);
            collection.Var_Luck = new RealStatVariable(0.25, 0.5, 1.0);

            collection.RedAffinityRate = 0.08;
            collection.GreenAffinityRate = 0.06;
            collection.BlueAffinityRate = 0.06;
            collection.PrismaticAffinityRate = 0.0;
            collection.VoidAffinityRate = 0.0;

            collection.Var_RedAffinity = new RealStatVariable(0.3, 1.25, 6.0);
            collection.Var_GreenAffinity = new RealStatVariable(0.15, 0.75, 4.0);
            collection.Var_BlueAffinity = new RealStatVariable(0.15, 0.75, 4.0);
            collection.Var_PrismaticAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_VoidAffinity = new RealStatVariable(0, 0, 0);

            var categoryModifiers = new List<WeightedItem<CategoryModifier>>
            {
                WeightedItem.Create(CategoryModifier.Child_Of_Din, 50.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Din, 10.0),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Din, 2.0),

                WeightedItem.Create(CategoryModifier.Child_Of_Farore, 15.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Farore, 2.5),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Farore, 0.5),

                WeightedItem.Create(CategoryModifier.Child_Of_Nayru, 15.0),
                WeightedItem.Create(CategoryModifier.Favored_Of_Nayru, 2.5),
                WeightedItem.Create(CategoryModifier.Blessed_Of_Nayru, 0.5),

                WeightedItem.Create(CategoryModifier.Hearty, 75.0),
                WeightedItem.Create(CategoryModifier.Imposing, 5.0),
                WeightedItem.Create(CategoryModifier.Commanding, 3.0),
                WeightedItem.Create(CategoryModifier.Modest, 12.5),
                WeightedItem.Create(CategoryModifier.Frail, 2.5),
                WeightedItem.Create(CategoryModifier.Feeble, 0.5),

                WeightedItem.Create(CategoryModifier.Charming, 25.0),
                WeightedItem.Create(CategoryModifier.Charismatic, 5.0),
                WeightedItem.Create(CategoryModifier.Enthralling, 1.0),
                WeightedItem.Create(CategoryModifier.Disagreeable, 25.0),
                WeightedItem.Create(CategoryModifier.Obnoxious, 5.0),
                WeightedItem.Create(CategoryModifier.Repulsive, 1.0),

                WeightedItem.Create(CategoryModifier.Adept, 35.0),
                WeightedItem.Create(CategoryModifier.Erudite, 15.0),
                WeightedItem.Create(CategoryModifier.Savant, 3.0),
                WeightedItem.Create(CategoryModifier.Unwieldy, 35.0),
                WeightedItem.Create(CategoryModifier.Inelegant, 15.0),
                WeightedItem.Create(CategoryModifier.Inept, 3.0),
            };

            collection.Var_CategoryModDistribution = new DistributionVariable<CategoryModifier>(categoryModifiers);

            var approachModifiers = new List<WeightedItem<ApproachModifier>>
            {
                WeightedItem.Create(ApproachModifier.Forceful, 20.0),
                WeightedItem.Create(ApproachModifier.Aggressive, 10.0),
                WeightedItem.Create(ApproachModifier.Vehement, 2.0),
                WeightedItem.Create(ApproachModifier.Passive, 0.5),
                WeightedItem.Create(ApproachModifier.Compliant, 0.25),
                WeightedItem.Create(ApproachModifier.Docile, 0.05),
                WeightedItem.Create(ApproachModifier.Decisive, 10.0),
                WeightedItem.Create(ApproachModifier.Determined, 5.0),
                WeightedItem.Create(ApproachModifier.Fateful, 1.0),
                WeightedItem.Create(ApproachModifier.Hesitant, 5.0),
                WeightedItem.Create(ApproachModifier.Reluctant, 2.5),
                WeightedItem.Create(ApproachModifier.Timid, 0.5),
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
