using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

using Highway.Data.Contexts.TypeRepresentations;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.Contexts
{
    public class InMemoryDataContext : IDataContext, IReadonlyDataContext
    {
        internal readonly ObjectRepresentationRepository Repo;

        private readonly Queue _addQueue = new Queue();

        private readonly Queue _removeQueue = new Queue();

        public InMemoryDataContext()
        {
            Repo = new ObjectRepresentationRepository();
            RegisterIIdentifiables();
        }

        internal InMemoryDataContext(ObjectRepresentationRepository repo)
        {
            Repo = repo;
            RegisterIIdentifiables();
        }

        public event EventHandler<BeforeSave> BeforeSave;

        public event EventHandler<AfterSave> AfterSave;

        public virtual T Add<T>(T item)
            where T : class
        {
            _addQueue.Enqueue(item);

            return item;
        }

        public virtual IQueryable<T> AsQueryable<T>()
            where T : class
        {
            return Repo.Data<T>();
        }

        public virtual int Commit()
        {
            OnBeforeSave(new BeforeSave());
            ProcessCommitQueues();
            Repo.Commit();
            OnAfterSave(new AfterSave());

            return 0;
        }

        public virtual Task<int> CommitAsync()
        {
            var task = new Task<int>(Commit);
            task.Start();

            return task;
        }

        public void Dispose()
        {
        }

        /// <summary>
        ///     This method allows you to register database "identity" like strategies for auto incrementing keys, or new guid
        ///     keys, etc...
        /// </summary>
        /// <param name="identityStrategy">The strategy to use for an object</param>
        /// <typeparam name="T">The type to use it from</typeparam>
        public void RegisterIdentityStrategy<T>(IIdentityStrategy<T> identityStrategy)
            where T : class
        {
            if (Repo.IdentityStrategies.ContainsKey(typeof(T)))
            {
                Repo.IdentityStrategies[typeof(T)] = obj => identityStrategy.Assign((T)obj);
            }
            else
            {
                Repo.IdentityStrategies.Add(typeof(T), obj => identityStrategy.Assign((T)obj));
            }
        }

        public virtual T Reload<T>(T item)
            where T : class
        {
            return item;
        }

        public virtual T Remove<T>(T item)
            where T : class
        {
            _removeQueue.Enqueue(item);

            return item;
        }

        public virtual T Update<T>(T item)
            where T : class
        {
            return item;
        }

        protected virtual void OnAfterSave(AfterSave e)
        {
            var handler = AfterSave;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBeforeSave(BeforeSave e)
        {
            var handler = BeforeSave;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        ///     Processes the held but uncommitted adds and removes from the context
        /// </summary>
        protected void ProcessCommitQueues()
        {
            AddAllFromQueueIntoRepository();
            RemoveAllFromQueueFromRepository();
        }

        private void AddAllFromQueueIntoRepository()
        {
            while (_addQueue.Count > 0)
            {
                Repo.Add(_addQueue.Dequeue());
            }
        }

        private void RegisterIIdentifiables()
        {
            RegisterIdentityStrategy(new IntegerIdentityStrategy<IIdentifiable<int>>(x => x.Id));
            RegisterIdentityStrategy(new ShortIdentityStrategy<IIdentifiable<short>>(x => x.Id));
            RegisterIdentityStrategy(new LongIdentityStrategy<IIdentifiable<long>>(x => x.Id));
            RegisterIdentityStrategy(new GuidIdentityStrategy<IIdentifiable<Guid>>(x => x.Id));
        }

        private void RemoveAllFromQueueFromRepository()
        {
            while (_removeQueue.Count > 0)
            {
                Repo.Remove(_removeQueue.Dequeue());
            }
        }
    }
}
