using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.Variables
{
    public class RealStatVariable
    {
        private SkewNormal Variable { get; }

        public RealStatVariable(double minimum, double mode, double maximum)
        {
            Variable = new SkewNormal(minimum, maximum, mode);
        }

        public double Sample(double weight, ref double aggregateVariance)
        {
            if (Variable.Min == Variable.Mode && Variable.Mode == Variable.Max)
            {
                return Variable.Mode;
            }

            var sample = Variable.Sample();

            var range = Variable.Max - Variable.Min;
            var dist = sample - Variable.Min;
            var variance = dist / range;
            aggregateVariance += variance * weight;

            return sample;
        }
    }
}
