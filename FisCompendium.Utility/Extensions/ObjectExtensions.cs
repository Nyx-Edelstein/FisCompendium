namespace FisCompendium.Utility.Extensions
{
    public static class ObjectExtensions
    {
        public static int ToInt(this object o)
        {
            var str = (o as string) ?? o.ToString();
            int.TryParse(str, out var i);
            return i;
        }

        public static double ToDouble(this object o)
        {
            var str = (o as string) ?? o.ToString();
            double.TryParse(str, out var i);
            return i;
        }
    }
}
