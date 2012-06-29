using System.Collections.Generic;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// An Interface for Queries that return collections
    /// </summary>
    /// <typeparam name="T">The Type being requested</typeparam>
    public interface IQuery<out T> : IQueryBase
    {
        /// <summary>
        /// This executes the expression in ContextQuery on the context that is passed in, resulting in a IQueryable<typeparam name="T"></typeparam> that is returned as an IEnumerable<typeparam name="T"></typeparam>
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>The collection of <typeparamref name="T"/> that the query materialized if any</returns>
        IEnumerable<T> Execute(IDataContext context);
    }
}
