namespace FisCompendium.Utility
{
    public class WeightedItem<T>
    {
        public T Item { get; set; }
        public double Weight { get; set; }

    }

    public static class WeightedItem
    {
        public static WeightedItem<T> Create<T>(T item, double weight)
        {
            return new WeightedItem<T>
            {
                Item = item,
                Weight = weight
            };
        }
    }
}
