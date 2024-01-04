using System;
using System.Collections.Generic;
using System.Linq;
using FisCompendium.Utility;

namespace FisCompendium.CharGen.Models.Variables
{
    public class DistributionVariable<T>
    {
        public List<WeightedItem<T>> Items { get; }

        public DistributionVariable(List<WeightedItem<T>> items)
        {
            Items = items;
        }

        public T SampleWhere(Func<T, bool> filter)
        {
            var filteredItems = Items.Where(x => filter(x.Item));
            return RNG.SelectWeighted(filteredItems.ToArray());
        }

        public List<T> SampleWhere(Func<T, bool> filter, int numToSample)
        {
            if (numToSample > Items.Count) numToSample = Items.Count;

            var selected = new HashSet<T>();
            for (var i = 0; i < numToSample; i++)
            {
                selected.Add(SampleWhere(x => !selected.Contains(x) && filter(x)));
            }

            return selected.ToList();
        }
    }
}
