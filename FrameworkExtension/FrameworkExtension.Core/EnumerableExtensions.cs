using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkExtension.Core
{
    public static class numerableExtensions
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