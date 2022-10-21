using System.Collections.Generic;

namespace Fizz6.Core
{
    public static class ListExt
    {
        public static bool Random<T>(this List<T> list, out T result)
        {
            if (list.Count == 0)
            {
                result = default;
                return false;
            }
            
            var index = UnityEngine.Random.Range(0, list.Count);
            result = list[index];
            return true;
        }

        public static T Random<T>(this List<T> list, IEnumerable<T> except = null) where T : class
        {
            if (list.Count == 0) return null;
            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }
    }
}