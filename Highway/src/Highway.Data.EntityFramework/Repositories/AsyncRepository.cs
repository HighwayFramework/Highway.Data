using System.Collections.Generic;
using System.Threading.Tasks;
using Highway.Data.Interfaces;

namespace Highway.Data.Repositories
{
    /// <summary>
    /// A Repository that implements a default Task based Async Pattern
    /// </summary>
    public class AsyncRepository : IAsyncRepository
    {
        public AsyncRepository(IDataContext context)
        {
            Context = context;
        }

        public IDataContext Context { get; private set; }
        public IEventManager EventManager { get; private set; }
        public Task<IEnumerable<T>> Find<T>(IQuery<T> query) where T : class
        {
            var task = new Task<IEnumerable<T>>(() =>
                {
                    lock (Context)
                    {
                        return query.Execute(Context);
                    }
                });
            return task;
        }

        public Task<T> Get<T>(IScalarObject<T> query)
        {
            var task = new Task<T>(() =>
            {
                lock (Context)
                {
                    return query.Execute(Context);
                }
            });
            return task;
        }

        public Task Execute(ICommandObject command)
        {
            var task = new Task(() =>
            {
                lock (Context)
                {
                    command.Execute(Context);
                }
            });
            return task;
        }
    }
}