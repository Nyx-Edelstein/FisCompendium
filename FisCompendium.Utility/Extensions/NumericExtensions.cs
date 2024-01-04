namespace FisCompendium.Utility.Extensions
{
    public static class NumericExtensions
    {
        public static double Bound(this double d, double min, double max)
        {
            return d < min ? min
                : d > max ? max
                : d;
        }
    }
}
