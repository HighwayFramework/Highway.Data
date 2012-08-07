using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// An Interface for Queries that return tasks of collections
    /// </summary>
    /// <typeparam name="T">The Type being requested</typeparam>
    public interface IAsyncQuery<T> : IQueryBase
    {
        /// <summary>
        /// This creates a task that executes the expression in ContextQuery on the context that is passed in, resulting in a <see cref="IQueryable{T}"/> that is returned as an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>A <see cref="Task{T}"/> that returns a collection of <typeparamref name="T"/> that the query materialized if any</returns>
        Task<IEnumerable<T>> Execute(IDataContext context);
    }
}