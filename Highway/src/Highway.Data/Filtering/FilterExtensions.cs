using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace Highway.Data.Filtering
{
    /// <summary>
    /// Basic Extensions for building the Filtering API
    /// </summary>
    public static class FilterExtensions
    {
        public static IEnumerable<T> FilterBy<T>(this IEnumerable<T> items, Criteria criteria)
        {
            return items.Where(criteria.GetFilterString(), criteria.GetFilterArguments());
        }

        public static IQueryable<T> FilterBy<T>(this IQueryable<T> items, Criteria criteria)
        {
            return items.Where(criteria.GetFilterString(), criteria.GetFilterArguments());
        }

        public static Criteria And(this Criteria leftCritera, Criteria rightCriteria)
        {
            return new AndCriteria(leftCritera,rightCriteria);
        }

        public static Criteria Or(this Criteria leftCriteria, Criteria rightCriteria)
        {
            return new OrCriteria(leftCriteria,rightCriteria);
        }
    }
}