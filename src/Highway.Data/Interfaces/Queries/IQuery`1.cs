using System.Collections.Generic;
using System.Linq;

namespace Highway.Data
{
    /// <summary>
    ///     An Interface for Queries that return collections
    /// </summary>
    /// <typeparam name="T">The Type being requested</typeparam>
    public interface IQuery<out T> : IQueryBase
    {
        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>The collection of <typeparamref name="T" /> that the query materialized if any</returns>
        IEnumerable<T> Execute(IReadonlyDataContext context);
    }
}
