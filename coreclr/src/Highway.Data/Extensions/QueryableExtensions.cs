using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Distinct<T, TGroup>(this IQueryable<T> source, Func<T, TGroup> grouping)
        {
            // passing the FuncComparer with the lamda to the 2nd overload of the Distinct method
            return source.GroupBy(grouping).Select(x => x.First()).AsQueryable();
        }
    }

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
    }
}
