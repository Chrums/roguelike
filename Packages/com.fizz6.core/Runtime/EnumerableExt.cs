using System;
using System.Collections.Generic;
using System.Linq;

namespace Fizz6.Core
{
    public static class EnumerableExt
    {
        private static readonly Random Randomizer = new();
        
        public static bool Random<T>(this IEnumerable<T> enumerable, out T result) => 
            enumerable.ToList().Random(out result);

        public static T Random<T>(this IEnumerable<T> enumerable) where T : class =>
            enumerable.ToList().Random();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) =>
            enumerable
                .OrderBy(value => Randomizer.Next())
                .ToList();

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action?.Invoke(item);
            return enumerable;
        }
    }
}