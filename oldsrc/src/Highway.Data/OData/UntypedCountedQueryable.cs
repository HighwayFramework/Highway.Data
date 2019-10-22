using System;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.OData
{
    /// <summary>
    /// A Queryable that returns object, but has the count of the underlying set, and the meta data for the type under query
    /// </summary>
    /// <typeparam name="T">The type under query</typeparam>
    public class UntypedCountedQueryable<T> : UntypedQueryable<T>
    {
        /// <summary>
        /// Creates an untyped queryable from a typed queryable, a projection expression, and the count of the underlying set
        /// </summary>
        /// <param name="source">The typed queryable to excute</param>
        /// <param name="selectExpression">the project to mutate the results with</param>
        /// <param name="inlineCount">The count before paging and projection</param>
        public UntypedCountedQueryable(IQueryable<T> source, Expression<Func<T, object>> selectExpression, int inlineCount)
            : base(source, selectExpression)
        {
            InlineCount = inlineCount;
        }

        /// <summary>
        /// The count of the underlying set before paging and projection.
        /// </summary>
        public int InlineCount { get; private set; }
    }
}