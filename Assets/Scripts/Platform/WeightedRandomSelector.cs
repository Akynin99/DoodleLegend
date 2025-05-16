using System.Collections.Generic;
using UnityEngine;

namespace DoodleLegend.Platform
{
    public class WeightedRandomSelector<T>
    {
        private struct Item
        {
            public T Value;
            public float Weight;
        }

        private List<Item> _items = new();
        private float _totalWeight;

        public void Add(T value, float weight)
        {
            _items.Add(new Item { Value = value, Weight = weight });
            _totalWeight += weight;
        }

        public T GetRandom()
        {
            float random = Random.Range(0, _totalWeight);
            foreach (var item in _items)
            {
                if (random < item.Weight)
                {
                    return item.Value;
                }
                random -= item.Weight;
            }
            return default;
        }

        public void Clear()
        {
            _items.Clear();
            _totalWeight = 0;
        }
    }
}