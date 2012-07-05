using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data
{
    /// <summary>
    /// Extensions to make dealing with enumerable collections easier
    /// </summary>
    public static class EnumerableExtensions
    {
         /// <summary>
         /// Executes an action for each item in an enumerable collection, this breaks defered execution
         /// </summary>
         /// <param name="items">The collection to be executed against</param>
         /// <param name="action">The action to be executed against the collection</param>
         /// <typeparam name="T">The type of objects in the collection</typeparam>
         /// <returns>The collection after the execution occurs</returns>
         public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action)
         {
             var itemsInArray = items.ToArray();
             foreach (var item in itemsInArray)
             {
                 action(item);
             }
             return itemsInArray;
         }
    }
}