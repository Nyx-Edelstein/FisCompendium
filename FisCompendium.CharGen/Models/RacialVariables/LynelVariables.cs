using System.Collections.Generic;
using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Interfaces;
using FisCompendium.CharGen.Models.Variables;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.RacialVariables
{
    public class LynelVariables : IRacialVariablesSelector
    {
        public VariableCollection Select()
        {
            var collection = new VariableCollection();

            collection.Var_Phys_Recovery = new IntegerStatVariable(12, 15, 24);
            collection.Var_Soc_Recovery = new IntegerStatVariable(3, 5, 6);
            collection.Var_Mag_Recovery = new IntegerStatVariable(3, 4, 6);

            collection.Var_Phys_MaxStress = new IntegerStatVariable(30, 45, 60);
            collection.Var_Soc_MaxStress = new IntegerStatVariable(3, 6, 10);
            collection.Var_Mag_MaxStress = new IntegerStatVariable(15, 22, 30);

            collection.Var_FateCap = new IntegerStatVariable(8, 10, 12);
            collection.Var_Luck = new RealStatVariable(0.25, 0.5, 1.0);

            collection.RedAffinityRate = 0.25;
            collection.GreenAffinityRate = 0.0;
            collection.BlueAffinityRate = 0.0;
            collection.PrismaticAffinityRate = 0.0;
            collection.VoidAffinityRate = 0.0;

            collection.Var_RedAffinity = new RealStatVariable(0.75, 3.0, 20.0);
            collection.Var_GreenAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_BlueAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_PrismaticAffinity = new RealStatVariable(0, 0, 0);
            collection.Var_VoidAffinity = new RealStatVariable(0, 0, 0);

            var categoryModifiers = new List<WeightedItem<CategoryModifier>>
            {
                WeightedItem.Create(CategoryModifier.Child_Of_Din, 10.0),

                WeightedItem.Create(CategoryModifier.Hearty, 10.0),
                WeightedItem.Create(CategoryModifier.Imposing, 5.0),
                WeightedItem.Create(CategoryModifier.Commanding, 2.5),

                WeightedItem.Create(CategoryModifier.Charming, 5.0),
                WeightedItem.Create(CategoryModifier.Charismatic, 2.5),
                WeightedItem.Create(CategoryModifier.Enthralling, 1.0),
                WeightedItem.Create(CategoryModifier.Disagreeable, 20.0),
                WeightedItem.Create(CategoryModifier.Obnoxious, 10.0),
                WeightedItem.Create(CategoryModifier.Repulsive, 5.0),

                WeightedItem.Create(CategoryModifier.Adept, 10.0),
                WeightedItem.Create(CategoryModifier.Unwieldy, 10.0),
            };

            collection.Var_CategoryModDistribution = new DistributionVariable<CategoryModifier>(categoryModifiers);

            var approachModifiers = new List<WeightedItem<ApproachModifier>>
            {
                WeightedItem.Create(ApproachModifier.Forceful, 100.0),
                WeightedItem.Create(ApproachModifier.Aggressive, 50.0),
                WeightedItem.Create(ApproachModifier.Vehement, 25.0),

                WeightedItem.Create(ApproachModifier.Decisive, 30.0),
                WeightedItem.Create(ApproachModifier.Determined, 15.0),
                WeightedItem.Create(ApproachModifier.Fateful, 3.0),
                
                WeightedItem.Create(ApproachModifier.Flashy, 10.0),
                WeightedItem.Create(ApproachModifier.Lively, 5.0),
                WeightedItem.Create(ApproachModifier.Exuberant, 1.0),
                WeightedItem.Create(ApproachModifier.Restrained, 10.0),
                WeightedItem.Create(ApproachModifier.Reticent, 5.0),
                WeightedItem.Create(ApproachModifier.Taciturn, 1.0),

                WeightedItem.Create(ApproachModifier.Perceptive, 25.0),
                WeightedItem.Create(ApproachModifier.Instinctive, 5.0),
                WeightedItem.Create(ApproachModifier.Careless, 25.0),
                WeightedItem.Create(ApproachModifier.Reckless, 5.0),

                WeightedItem.Create(ApproachModifier.Disorganized, 10.0),
                WeightedItem.Create(ApproachModifier.Undisciplined, 5.0),
                WeightedItem.Create(ApproachModifier.Haphazard, 1.0),

                WeightedItem.Create(ApproachModifier.Slow, 50.0),
                WeightedItem.Create(ApproachModifier.Dull, 25.0),
                WeightedItem.Create(ApproachModifier.Simple, 5.0),
            };

            collection.Var_ApproachModDistribution = new DistributionVariable<ApproachModifier>(approachModifiers);

            return collection;
        }
    }
}
