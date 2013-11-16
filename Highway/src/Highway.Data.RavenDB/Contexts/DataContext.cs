using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.Interceptors.Events;
using Highway.Data;
using Raven.Client;

namespace Highway.Data
{
    /// <summary>
    /// A base implementation of the RavenDB DataContext
    /// </summary>
    public partial class DataContext : IDataContext
    {
        private readonly ILog _log;

        public DataContext(IDocumentSession session, ILog log)
        {
            _log = log;
            _session = session;
        }

        /// <summary>
        /// This gives a mockable wrapper around the normal <see cref="DbSet{T}"/> method that allows for testablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns><see cref="IQueryable{T}"/></returns>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            _log.DebugFormat("Querying Object {0}", typeof (T).Name);
            IQueryable<T> result = Query<T>();
            _log.DebugFormat("Queried Object {0}", typeof (T).Name);
            return result;
        }

        /// <summary>
        /// Adds the provided instance of <typeparamref name="T"/> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being added</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to add</param>
        /// <returns>The <typeparamref name="T"/> you added</returns>
        public T Add<T>(T item) where T : class
        {
            _log.DebugFormat("Adding Object {0}", item);
            Store(item);
            _log.TraceFormat("Added Object {0}", item);
            return item;
        }

        /// <summary>
        /// Removes the provided instance of <typeparamref name="T"/> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being removed</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to remove</param>
        /// <returns>The <typeparamref name="T"/> you removed</returns>
        public T Remove<T>(T item) where T : class
        {
            _log.DebugFormat("Removing Object {0}", item);
            Delete(item);
            _log.TraceFormat("Removed Object {0}", item);
            return item;
        }

        /// <summary>
        /// Updates the provided instance of <typeparamref name="T"/> in the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being updated</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to update</param>
        /// <returns>The <typeparamref name="T"/> you updated</returns>
        public T Update<T>(T item) where T : class
        {
            _log.DebugFormat("Updating Object {0}", item);
            Store(item);
            _log.TraceFormat("Updated Object {0}", item);
            return item;
        }

        /// <summary>
        /// Reloads the provided instance of <typeparamref name="T"/> from the database
        /// </summary>
        /// <typeparam name="T">The Entity Type being reloaded</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to reload</param>
        /// <returns>The <typeparamref name="T"/> you reloaded</returns>
        public T Reload<T>(T item) where T : class
        {
            _log.DebugFormat("Reloading Object {0}", item);
            var id = Advanced.GetDocumentId(item);
            item = Load<T>(id);
            _log.TraceFormat("Reloaded Object {0}", item);
            return item;
        }

        /// <summary>
        /// Commits all currently tracked entity changes
        /// </summary>
        /// <returns>the number of rows affected</returns>
        public int Commit()
        {
            _log.Trace("\tCommit");
            SaveChanges();
            _log.DebugFormat("\tCommited Changes");
            return 0;
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}