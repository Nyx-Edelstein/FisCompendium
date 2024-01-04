using System;
using System.Linq;
using FisCompendium.CharGen.Attributes;
using FisCompendium.CharGen.Data;
using FisCompendium.CharGen.Enum;
using FisCompendium.Utility;
using FisCompendium.Utility.Extensions;

namespace FisCompendium.CharGen.Models
{
    internal class VariableSampler
    {
        private VariableCollection Variables { get; }
        public Race Race { get; }
        private int Age { get; }
        private double XpCoeff { get; }

        public VariableSampler(VariableCollection racialVariables, Race race, int age, double xpCoeff)
        {
            Variables = racialVariables;
            Race = race;
            Age = age;
            XpCoeff = xpCoeff;
        }

        public GridData GetSampleData()
        {
            var aggregateVariance = 0.0;

            var data = new GridData();
            data.Phys_Recovery = Variables.Var_Phys_Recovery.Sample(5, ref aggregateVariance, inverted: true);
            data.Soc_Recovery = Variables.Var_Soc_Recovery.Sample(5, ref aggregateVariance, inverted: true);
            data.Mag_Recovery = Variables.Var_Mag_Recovery.Sample(5, ref aggregateVariance, inverted: true);
            data.Phys_MaxStress = Variables.Var_Phys_MaxStress.Sample(5, ref aggregateVariance);
            data.Soc_MaxStress = Variables.Var_Soc_MaxStress.Sample(5, ref aggregateVariance);
            data.Mag_MaxStress = Variables.Var_Mag_MaxStress.Sample(5, ref aggregateVariance);
            data.FPCap = Variables.Var_FateCap.Sample(10, ref aggregateVariance);
            data.Luck = Variables.Var_Luck.Sample(10, ref aggregateVariance);

            data.RedAffinity = RNG.NextDouble() < Variables.RedAffinityRate ? Variables.Var_RedAffinity.Sample(15, ref aggregateVariance) : 0.0;
            data.GreenAffinity = RNG.NextDouble() < Variables.GreenAffinityRate ? Variables.Var_GreenAffinity.Sample(15, ref aggregateVariance) : 0.0;
            data.BlueAffinity = RNG.NextDouble() < Variables.BlueAffinityRate ? Variables.Var_BlueAffinity.Sample(15, ref aggregateVariance) : 0.0;

            var affinities = 0.0;

            if (data.RedAffinity > 1) affinities += 1;
            else affinities += Math.Pow(data.RedAffinity, 3);

            if (data.GreenAffinity > 1) affinities += 1;
            else affinities += Math.Pow(data.GreenAffinity, 3);

            if (data.BlueAffinity > 1) affinities += 1;
            else affinities += Math.Pow(data.BlueAffinity, 3);

            aggregateVariance += Math.Pow(Math.E, affinities);

            data.HasEndowment = RNG.NextBoolean() ? 1 : 0;
            data.HasWeakness = RNG.NextBoolean() ? 1 : 0;
            data.NumBonuses = RNG.Next(0, 3);
            data.NumMaluses = RNG.Next(0, 3);
            data.NumNarrative = RNG.Next(1, 2);
            data.NumProfessions = RNG.Next(1, 2);

            if (data.HasEndowment == 1) DetermineEndowment(data, ref aggregateVariance);
            if (data.HasWeakness == 1) DetermineWeakness(data, ref aggregateVariance);
            if (data.NumBonuses > 0) DetermineBonuses(data, ref aggregateVariance);
            if (data.NumMaluses > 0) DetermineMaluses(data, ref aggregateVariance);

            DetermineApproach(data);
            data.XPInitial = GetXP(Race, Age, XpCoeff);
            data.Variance = aggregateVariance;

            return data;
        }

        private void DetermineEndowment(GridData data, ref double aggregateVariance)
        {
            var endowment = Variables.Var_CategoryModDistribution
                .SampleWhere(x =>
                {
                    var modifier = x.GetAttributeOfType<CategoryModifierAttribute>();
                    return modifier.Shifts > 0 && (data.RedAffinity > 0 || modifier.Category != Category.Red) && (data.GreenAffinity > 0 || modifier.Category != Category.Green) && (data.BlueAffinity > 0 || modifier.Category != Category.Blue);
                });
            data.Endowment = $"{endowment} ({endowment.GetAttributeOfType<CategoryModifierAttribute>().Description})";

            HandleCategoryModifier(data, endowment, ref aggregateVariance);
        }

        private void DetermineWeakness(GridData data, ref double aggregateVariance)
        {
            //If there isn't an endowment, there are no restrictions here
            if (string.IsNullOrWhiteSpace(data.Endowment))
            {
                var w = Variables.Var_CategoryModDistribution
                    .SampleWhere(x => x.GetAttributeOfType<CategoryModifierAttribute>().Shifts < 0);
                data.Weakness = $"{w} ({w.GetAttributeOfType<CategoryModifierAttribute>().Description})";

                HandleCategoryModifier(data, w, ref aggregateVariance);

                return;
            }

            //Otherwise we need to ensure it doesn't conflict with the endowment
            var endowment = data.Endowment.Substring(0, data.Endowment.IndexOf(" "));
            var endowmentCategory = System.Enum.Parse<CategoryModifier>(endowment)
                .GetAttributeOfType<CategoryModifierAttribute>()
                .Category;

            var weakness = Variables.Var_CategoryModDistribution
                .SampleWhere(x =>
                {
                    var modifier = x.GetAttributeOfType<CategoryModifierAttribute>();
                    return modifier.Category != endowmentCategory && modifier.Shifts < 0;
                });

            data.Weakness = $"{weakness} ({weakness.GetAttributeOfType<CategoryModifierAttribute>().Description})";

            HandleCategoryModifier(data, weakness, ref aggregateVariance);
        }

        private void DetermineBonuses(GridData data, ref double aggregateVariance)
        {
            var bonuses = Variables.Var_ApproachModDistribution
                .SampleWhere(x => x.GetAttributeOfType<ApproachModifierAttribute>().Shifts > 0, numToSample: data.NumBonuses)
                .Select(x => new
                {
                    modifier = x.GetAttributeOfType<ApproachModifierAttribute>(),
                    data = x
                })
                .GroupBy(x => x.modifier.Approach)
                .Select(g => g.OrderByDescending(x => x.modifier.Shifts).First().data)
                .ToList();

            if (bonuses.Count > 0)
            {
                var bonus = bonuses[0];
                data.Bonus1 = $"{bonus} ({bonus.GetAttributeOfType<ApproachModifierAttribute>().Description})";
                HandleApproachModifier(data, bonus, ref aggregateVariance);
            }

            if (bonuses.Count > 1)
            {
                var bonus = bonuses[1];
                data.Bonus2 = $"{bonus} ({bonus.GetAttributeOfType<ApproachModifierAttribute>().Description})";
                HandleApproachModifier(data, bonus, ref aggregateVariance);
            }

            if (bonuses.Count > 2)
            {
                var bonus = bonuses[2];
                data.Bonus3 = $"{bonus} ({bonus.GetAttributeOfType<ApproachModifierAttribute>().Description})";
                HandleApproachModifier(data, bonus, ref aggregateVariance);
            }
        }

        private void DetermineMaluses(GridData data, ref double aggregateVariance)
        {
            var bonuses = new[]
            {
                data.Bonus1 ?? string.Empty,
                data.Bonus2 ?? string.Empty,
                data.Bonus3 ?? string.Empty,
            }.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x =>
                {
                    var approach = x.Substring(0, x.IndexOf(" "));
                    return System.Enum.Parse<ApproachModifier>(approach).GetAttributeOfType<ApproachModifierAttribute>().Approach;
                });

            var maluses = Variables.Var_ApproachModDistribution
                .SampleWhere(x =>
                {
                    var modifier = x.GetAttributeOfType<ApproachModifierAttribute>();
                    return !bonuses.Contains(modifier.Approach) && modifier.Shifts < 0;
                }, data.NumMaluses)
                .Select(x => new
                {
                    modifier = x.GetAttributeOfType<ApproachModifierAttribute>(),
                    data = x
                })
                .GroupBy(x => x.modifier.Approach)
                .Select(g => g.OrderBy(x => x.modifier.Shifts).First().data)
                .ToList();

            if (maluses.Count > 0)
            {
                var malus = maluses[0];
                data.Malus1 = $"{malus} ({malus.GetAttributeOfType<ApproachModifierAttribute>().Description})";
                HandleApproachModifier(data, malus, ref aggregateVariance);
            }

            if (maluses.Count > 1)
            {
                var malus = maluses[1];
                data.Malus2 = $"{malus} ({malus.GetAttributeOfType<ApproachModifierAttribute>().Description})";
                HandleApproachModifier(data, malus, ref aggregateVariance);
            }

            if (maluses.Count > 2)
            {
                var malus = maluses[2];
                data.Malus3 = $"{malus} ({malus.GetAttributeOfType<ApproachModifierAttribute>().Description})";
                HandleApproachModifier(data, malus, ref aggregateVariance);
            }
        }

        private static void HandleCategoryModifier(GridData data, CategoryModifier modifier, ref double aggregateVariance)
        {
            var attr = modifier.GetAttributeOfType<CategoryModifierAttribute>();
            var category = attr.Category;
            var shifts = attr.Shifts;

            aggregateVariance += shifts * 6;

            switch (category)
            {
                case Category.Red:
                    data.Phys_Forceful += shifts;
                    data.Phys_Decisive += shifts;
                    data.Soc_Forceful += shifts;
                    data.Soc_Decisive += shifts;
                    data.Mag_Forceful += shifts;
                    data.Mag_Decisive += shifts;
                    break;
                case Category.Green:
                    data.Phys_Flashy += shifts;
                    data.Phys_Intuitive += shifts;
                    data.Soc_Flashy += shifts;
                    data.Soc_Intuitive += shifts;
                    data.Mag_Flashy += shifts;
                    data.Mag_Intuitive += shifts;
                    break;
                case Category.Blue:
                    data.Phys_Methodical += shifts;
                    data.Phys_Clever += shifts;
                    data.Soc_Methodical += shifts;
                    data.Soc_Clever += shifts;
                    data.Mag_Methodical += shifts;
                    data.Mag_Clever += shifts;
                    break;
                case Category.Physical:
                    data.Phys_Forceful += shifts;
                    data.Phys_Decisive += shifts;
                    data.Phys_Flashy += shifts;
                    data.Phys_Intuitive += shifts;
                    data.Phys_Methodical += shifts;
                    data.Phys_Clever += shifts;
                    break;
                case Category.Social:
                    data.Soc_Forceful += shifts;
                    data.Soc_Decisive += shifts;
                    data.Soc_Flashy += shifts;
                    data.Soc_Intuitive += shifts;
                    data.Soc_Methodical += shifts;
                    data.Soc_Clever += shifts;
                    break;
                case Category.Magical:
                    data.Mag_Forceful += shifts;
                    data.Mag_Decisive += shifts;
                    data.Mag_Flashy += shifts;
                    data.Mag_Intuitive += shifts;
                    data.Mag_Methodical += shifts;
                    data.Mag_Clever += shifts;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void HandleApproachModifier(GridData data, ApproachModifier modifier, ref double aggregateVariance)
        {
            var attr = modifier.GetAttributeOfType<ApproachModifierAttribute>();
            var approach = attr.Approach;
            var shifts = attr.Shifts;

            aggregateVariance += shifts * 3;

            switch (approach)
            {
                case Approaches.Forceful:
                    data.Phys_Forceful += shifts;
                    data.Soc_Forceful += shifts;
                    data.Mag_Forceful += shifts;
                    break;
                case Approaches.Decisive:
                    data.Phys_Decisive += shifts;
                    data.Soc_Decisive += shifts;
                    data.Mag_Decisive += shifts;
                    break;
                case Approaches.Flashy:
                    data.Phys_Flashy += shifts;
                    data.Soc_Flashy += shifts;
                    data.Mag_Flashy += shifts;
                    break;
                case Approaches.Intuitive:
                    data.Phys_Intuitive += shifts;
                    data.Soc_Intuitive += shifts;
                    data.Mag_Intuitive += shifts;
                    break;
                case Approaches.Methodical:
                    data.Phys_Methodical += shifts;
                    data.Soc_Methodical += shifts;
                    data.Mag_Methodical += shifts;
                    break;
                case Approaches.Clever:
                    data.Phys_Clever += shifts;
                    data.Soc_Clever += shifts;
                    data.Mag_Clever += shifts;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void DetermineApproach(GridData data)
        {
            var weightedApproaches = new[]
            {
                new { approach=Approaches.Forceful, weight=GetWeight(data.Phys_Forceful, data.Soc_Forceful, data.Forceful, data.RedAffinity)},
                new { approach=Approaches.Decisive, weight=GetWeight(data.Phys_Decisive, data.Soc_Decisive, data.Mag_Decisive, data.RedAffinity)},
                new { approach=Approaches.Flashy, weight=GetWeight(data.Phys_Flashy, data.Soc_Flashy, data.Mag_Flashy, data.GreenAffinity)},
                new { approach=Approaches.Intuitive, weight=GetWeight(data.Phys_Intuitive, data.Soc_Intuitive, data.Mag_Intuitive, data.GreenAffinity)},
                new { approach=Approaches.Methodical, weight=GetWeight(data.Phys_Methodical, data.Soc_Methodical, data.Mag_Methodical, data.BlueAffinity)},
                new { approach=Approaches.Clever, weight=GetWeight(data.Phys_Clever, data.Soc_Clever, data.Mag_Clever, data.BlueAffinity)}
            }.OrderByDescending(x => x.weight)
            .ToList();

            var approachStrategy = Strategies.RandomStrategy();

            for (var i = 0; i < weightedApproaches.Count; i++)
            {
                var x = weightedApproaches[i];
                switch (x.approach)
                {
                    case Approaches.Forceful:
                        data.Forceful = approachStrategy[i];
                        break;
                    case Approaches.Decisive:
                        data.Decisive = approachStrategy[i];
                        break;
                    case Approaches.Flashy:
                        data.Flashy = approachStrategy[i];
                        break;
                    case Approaches.Intuitive:
                        data.Intuitive = approachStrategy[i];
                        break;
                    case Approaches.Methodical:
                        data.Methodical = approachStrategy[i];
                        break;
                    case Approaches.Clever:
                        data.Clever = approachStrategy[i];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
        }

        private static double GetWeight(int a, int b, int c, double affinity)
        {
            var affinityModifier = affinity > 0 ? 0.85 : 1.0;
            return Math.Pow((a + 4) * (b + 4) * (c + 4) / affinityModifier, 1.0 / 3.0);
        }

        private int GetXP(Race race, double age, double xpCoeff)
        {
            var xpTotal = 0.0;
            var xpBreakpoints = race.GetAttributeOfType<RaceXPBreakpointsAttribute>();

            if (age < xpBreakpoints.Adolescence)
            {
                var maxXP = 2500.0;
                var fractionEarned = age / xpBreakpoints.Adolescence;
                var coeff = xpCoeff * 1.35;

                return (int)Math.Round(maxXP * fractionEarned * coeff);
            }
            else
            {
                var maxXP = 2500.0;
                var coeff = xpCoeff * 1.35;

                xpTotal += maxXP * coeff;
            }

            if (age >= xpBreakpoints.Adolescence && age < xpBreakpoints.Adulthood)
            {
                var maxXP = 4000.0;
                var fractionEarned = (age - xpBreakpoints.Adolescence) / (xpBreakpoints.Adulthood - xpBreakpoints.Adolescence);
                var coeff = xpCoeff * 1.15;

                xpTotal += maxXP * fractionEarned * coeff;
                return (int)Math.Round(xpTotal);
            }
            else
            {
                var maxXP = 4000.0;
                var coeff = xpCoeff * 1.15;

                xpTotal += maxXP * coeff;
            }

            if (age >= xpBreakpoints.Adulthood && age < xpBreakpoints.Mature)
            {
                var maxXP = 6500.0;
                var fractionEarned = (age - xpBreakpoints.Adulthood) / (xpBreakpoints.Mature - xpBreakpoints.Adulthood);
                var coeff = xpCoeff;

                xpTotal += maxXP * fractionEarned * coeff;
                return (int) Math.Round(xpTotal);
            }
            else
            {
                var maxXP = 6500.0;
                var coeff = xpCoeff;

                xpTotal += maxXP * coeff;
            }

            var daysPastMaturity = (age - xpBreakpoints.Mature) * 280;
            var coefficient = xpCoeff * 0.85 * xpBreakpoints.OldAgeCoefficient;

            xpTotal += daysPastMaturity * coefficient;

            return (int)Math.Round(xpTotal);
        }
    }
}