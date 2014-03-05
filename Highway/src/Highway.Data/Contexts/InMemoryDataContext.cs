#region

using System.Linq;
using Highway.Data.Contexts.TypeRepresentations;

#endregion

namespace Highway.Data.Contexts
{
    public class InMemoryDataContext : IDataContext
    {
        internal ObjectRepresentationRepository repo = new ObjectRepresentationRepository();

        public void Dispose()
        {
        }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return repo.Data<T>();
        }

        public T Add<T>(T item) where T : class
        {
            repo.Add(item);
            return item;
        }

        public T Remove<T>(T item) where T : class
        {
            repo.Remove(item);
            return item;
        }

        public T Update<T>(T item) where T : class
        {
            return item;
        }

        public T Reload<T>(T item) where T : class
        {
            return item;
        }

        public virtual int Commit()
        {
            repo.CleanGraph();
            repo.FindChanges();
            return 0;
        }

        /// <summary>
        /// This method allows you to register database "identity" like strategies for auto incrementing keys, or new guid keys, etc...
        /// </summary>
        /// <param name="integerIdentityStrategy">The strategy to use for an object</param>
        /// <typeparam name="T">The type to use it from</typeparam>
        public void RegisterIdentityStrategy<T>(IIdentityStrategy<T> integerIdentityStrategy) where T : class
        {
            if (repo.IdentityStrategies.ContainsKey(typeof (T)))
            {
                repo.IdentityStrategies[typeof (T)] = obj => integerIdentityStrategy.Assign((T) obj);
            }
            else
            {
                repo.IdentityStrategies.Add(typeof (T), obj => integerIdentityStrategy.Assign((T) obj));
            }
        }
    }
}