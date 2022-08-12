using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fizz6.Core
{
    public static class RandomExt
    {
        public static int WeightedRandom(this IList<float> items)
        {
            var aggregate = items.Aggregate(0.0f, (current, item) => current + item);
            var random = Random.Range(0.0f, aggregate);
            for (var index = 0; index < items.Count; ++index)
            {
                random -= items[index];
                if (random <= 0.0f) return index;
            }

            return 0;
        }
    }
}