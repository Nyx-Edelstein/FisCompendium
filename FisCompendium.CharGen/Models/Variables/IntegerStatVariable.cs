using System;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.Variables
{
    public class IntegerStatVariable
    {
        private SkewNormal Variable { get; }

        public IntegerStatVariable(int minimum, int mode, int maximum)
        {
            Variable = new SkewNormal(minimum, maximum, mode);
        }

        public int Sample(double weight, ref double aggregateVariance, bool inverted = false)
        {
            if (Variable.Min == Variable.Mode && Variable.Mode == Variable.Max)
            {
                return (int)Math.Round(Variable.Mode);
            }

            var sample = (int) Math.Round(Variable.Sample());
            var range = Variable.Max - Variable.Min;

            if (inverted)
            {
                var dist = Variable.Max - sample;
                var variance = dist / range;
                aggregateVariance += variance * weight;
            }
            else
            {
                var dist = sample - Variable.Min;
                var variance = dist / range;
                aggregateVariance += variance * weight;
            }

            return sample;
        }
    }
}
