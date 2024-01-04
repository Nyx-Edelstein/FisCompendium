using System;

namespace FisCompendium.CharGen.Attributes
{
    public class RaceXPBreakpointsAttribute : Attribute
    {
        public double Adolescence { get; }
        public double Adulthood { get; }
        public double Mature { get; }
        public double OldAgeCoefficient { get; }

        public RaceXPBreakpointsAttribute(double adolescence, double adulthood, double mature, double oldAgeCoefficient)
        {
            Adolescence = adolescence;
            Adulthood = adulthood;
            Mature = mature;
            OldAgeCoefficient = oldAgeCoefficient;
        }
    }
}
