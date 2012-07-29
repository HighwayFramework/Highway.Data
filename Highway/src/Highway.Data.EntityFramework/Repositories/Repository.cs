using System.Collections.Generic;
using Highway.Data.Interfaces;

namespace Highway.Data.Repositories
{
    /// <summary>
    /// A Entity Framework Specific repository implementation that uses Specification pattern to execute Queries in a controlled fashion.
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        /// Creates a Repository that uses the context provided
        /// </summary>
        /// <param name="context">The data context that this repository uses</param>
        public Repository(IDataContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Reference to the DataContext the repository is using
        /// </summary>
        public IDataContext Context { get; private set; }
        
        /// <summary>
        /// Reference to the EventManager the repository is using
        /// </summary>
        public IEventManager EventManager { get; private set; }

        /// <summary>
        /// Executes a prebuilt <see cref="IScalarObject{T}"/> and returns a single instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The instance of <typeparamref name="T"/> returned from the query</returns>
        public T Get<T>(IScalarObject<T> query)
        {
            return query.Execute(Context);
        }
        /// <summary>
        /// Executes a prebuilt <see cref="ICommandObject"/>
        /// </summary>
        /// <param name="command">The prebuilt command object</param>
        public void Execute(ICommandObject command)
        {
            command.Execute(Context);
        }
        /// <summary>
        /// Executes a prebuilt <see cref="IQuery{T}"/> and returns an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}"/> returned from the query</returns>
        public IEnumerable<T> Find<T>(IQuery<T> query) where T : class
        {
            return query.Execute(Context);
        }
    }
}