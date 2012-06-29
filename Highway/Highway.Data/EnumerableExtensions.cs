using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data
{
    public static class EnumerableExtensions
    {
         public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action)
         {
             var itemsInArray = items.ToArray();
             foreach (var item in itemsInArray)
             {
                 action(item);
             }
             return items;
         }
    }
}