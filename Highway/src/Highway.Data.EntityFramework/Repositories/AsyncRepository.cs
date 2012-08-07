using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Highway.Data.Interfaces;
using Highway.Data.QueryObjects;

namespace Highway.Data
{
    /// <summary>
    /// A Repository that implements a default Task based Async Pattern
    /// </summary>
    public class AsyncRepository : IAsyncRepository
    {
        /// <summary>
        /// Creates an Async Repository that uses Async and Await
        /// </summary>
        /// <param name="context">The data context being leveraged</param>
        public AsyncRepository(IDataContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Reference to the Context the repository is using
        /// </summary>
        public IDataContext Context { get; private set; }

        /// <summary>
        /// Reference to the EventManager the repository is using
        /// </summary>
        public IEventManager EventManager { get; private set; }

        /// <summary>
        /// Executes a prebuilt <see cref="IQuery{T}"/> and returns an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <param name="query">The prebuilt Query Object</param>
        /// <returns>The <see cref="IEnumerable{T}"/> returned from the query</returns>
        public Task<IEnumerable<T>> Find<T>(IQuery<T> query) where T : class
        {
            var asyncQuery = new AsyncQuery<T>(query);
            var task = asyncQuery.Execute(Context);
            task.Start();
            return task;
        }

        public async Task<T> Get<T>(IScalar<T> query)
        {
            var task = new Task<T>(() =>
            {
                lock (Context)
                {
                    return query.Execute(Context);
                }
            });
            return await task;
        }

        public async Task Execute(ICommand command)
        {
            var task = new Task(() =>
            {
                lock (Context)
                {
                    command.Execute(Context);
                }
            });
            await task;
        }
    }
}