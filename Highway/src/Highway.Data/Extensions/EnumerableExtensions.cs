using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Adds an Extension Method to Determine if a Collection has distinct values based on a property defined by a lambda expression
        /// </summary>
        /// <typeparam name="TSource">Collection</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">Lambda Expression</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        public static IEnumerable<TSource> DistinctBy<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> uniquenessComparison)
        {
            var list = new List<TSource>();
            return source.Where(element =>
            {
                var result = list.Any(source1 => uniquenessComparison(element, source1));
                list.Add(element);
                return result;
            });
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source)
            {
                action(t);
            }
        }
    }
}