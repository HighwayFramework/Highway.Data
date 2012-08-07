using System.Threading.Tasks;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// An Interface for Scalar Queries that return a single value or object
    /// </summary>
    /// <typeparam name="T">The Type that is being returned</typeparam>
    public interface IAsyncScalar<T>
    {
        /// <summary>
        /// Executes the expression against the passed in context
        /// </summary>
        /// <param name="context">The data context that the scalar query is executed against</param>
        /// <returns>The instance of <typeparamref name="T"/> that the query materialized if any</returns>
        Task<T> Execute(IDataContext context);
    }
}