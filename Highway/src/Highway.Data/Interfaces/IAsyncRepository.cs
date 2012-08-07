using System.Collections.Generic;
using System.Threading.Tasks;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// The interface used to interact with the ORM Specific Implementations
    /// </summary>
    public interface IAsyncRepository
    {
        /// <summary>
        /// Reference to the Context the repository is using
        /// </summary>
        IDataContext Context { get; }

        /// <summary>
        /// Reference to the EventManager the repository is using
        /// </summary>
        IEventManager EventManager { get; }

        /// <summary>
        /// Executes a prebuilt <see cref="IQuery{T}"/> and returns an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}"/> returned from the query</returns>
        Task<IEnumerable<T>> Find<T>(IQuery<T> query) where T : class;

        /// <summary>
        /// Executes a prebuilt <see cref="IScalar{T}"/> and returns a single instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The instance of <typeparamref name="T"/> returned from the query</returns>
        Task<T> Get<T>(IScalar<T> query);

        /// <summary>
        /// Executes a prebuilt <see cref="ICommand"/>
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        Task Execute(ICommand command);
    }
}