using System;
using System.Linq;

namespace Highway.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Distinct<T, TGroup>(this IQueryable<T> source, Func<T, TGroup> grouping)
        {
            // passing the FuncComparer with the lamda to the 2nd overload of the Distinct method
            return source.GroupBy(grouping).Select(x => x.First()).AsQueryable();
        }
    }
}