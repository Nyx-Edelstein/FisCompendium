using System;
using FisCompendium.Utility.Extensions;

namespace FisCompendium.Utility
{
    public class SkewNormal
    {
        public double Min { get; private set; }
        public double Max { get; private set; }
        public double Mode { get; private set; }
        public double Baseline { get; }

        public double? Value { get; private set; }

        public SkewNormal(double min, double max)
        {
            if (max <= min) throw new ArgumentException("max must be strictly greater than min");

            Min = min;
            Max = max;
            Mode = (max + min) / 2.0;
            Baseline = Mode;
        }

        public SkewNormal(double min, double max, double mode)
        {
            if (max < min) throw new ArgumentException("max must be strictly greater than min");

            Min = min;
            Max = max;
            Mode = mode.Bound(Min, Max);
            Baseline = Mode;
        }

        public double Sample()
        {
            const double tightness = 3;
            double candidate;

            do
            {
                if (RNG.NextBoolean())
                {
                    var std_dev = (Max - Mode) / tightness;
                    var rand = Math.Abs(RNG.NextGaussian(0, std_dev));
                    candidate = Mode + rand;
                }
                else
                {
                    var std_dev = (Mode - Min) / tightness;
                    var rand = Math.Abs(RNG.NextGaussian(0, std_dev));
                    candidate = Mode - rand;
                }
            } while (candidate < Min || candidate > Max);

            Value = candidate;
            return candidate;
        }
    }
}
