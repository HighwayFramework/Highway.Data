using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using Highway.Data.Interceptors.Events;
using Highway.Data.Interfaces;
using NHibernate;
using NHibernate.Event.Default;
using NHibernate.Linq;

namespace Highway.Data.NHibernate.Contexts
{
    /// <summary>
    /// A base implementation of Context that wraps an NHibernate Session
    /// </summary>
    public class Context : IObservableDataContext
    {
        private IEventManager _eventManager;
        private ISession _session;

        /// <summary>
        /// Constructs a context
        /// </summary>
        /// <param name="session">The NHibernate session that this context wraps </param>
        public Context(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// This gives a mockable wrapper around the normal <see cref="DbSet{T}"/> method that allows for testablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns><see cref="IQueryable{T}"/></returns>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return _session.Query<T>();
        }

        /// <summary>
        /// Adds the provided instance of <typeparamref name="T"/> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being added</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to add</param>
        /// <returns>The <typeparamref name="T"/> you added</returns>
        public T Add<T>(T item) where T : class
        {
            this._session.Persist(item);
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
            this._session.Delete(item);
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
            _session.Update(item);
            return item;
        }

        /// <summary>
        /// Attaches the provided instance of <typeparamref name="T"/> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to attach</param>
        /// <returns>The <typeparamref name="T"/> you attached</returns>
        public T Attach<T>(T item) where T : class
        {
            _session.Persist(item);
            return item;
        }

        /// <summary>
        /// Detaches the provided instance of <typeparamref name="T"/> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparamref name="T"/> you want to detach</param>
        /// <returns>The <typeparamref name="T"/> you detached</returns>
        public T Detach<T>(T item) where T : class
        {
            _session.Evict(item);
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
            _session.Refresh(item);
            return item;
        }

        /// <summary>
        /// Reloads all tracked objects of the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of objects to reload</typeparam>
        [Obsolete("This operation is not support on NHibernate")]
        public void Reload<T>() where T : class
        {
            //Not supported
            throw new NotSupportedException("The reload of a full set is not supported by NHibernate");
        }
        
        /// <summary>
        /// Commits all currently tracked entity changes
        /// </summary>
        /// <returns> Zero - nHibernate doesn't return record counts</returns>
        public int Commit()
        {
            InvokePreSave();
            _session.Flush();
            InvokePostSave();
            return 0;
        }

        private void InvokePostSave()
        {
            if (PostSave != null) PostSave(this, new PostSaveEventArgs());
        }

        private void InvokePreSave()
        {
            if (PreSave != null) PreSave(this, new PreSaveEventArgs(){});
        }

        /// <summary>
        /// Executes a SQL command and tries to map the returned datasets into an <see cref="IEnumerable{T}"/>
        /// The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}"/> from the query return</returns>
        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            throw new NotImplementedException("Not yet done");
        }

        /// <summary>
        /// Executes a SQL command and returns the standard int return from the query
        /// </summary>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            throw new NotImplementedException("Not yet done");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        public int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams)
        {
            throw new NotImplementedException("Not yet done");
        }

        
        /// <summary>
        /// The reference to EventManager that allows for ordered event handling and registration
        /// </summary>
        public IEventManager EventManager
        {
            get { return _eventManager; }
            set 
            { 
                _eventManager = value;
                _eventManager.Context = this;
            }
        }

        /// <summary>
        /// The event fired just before the commit of the ORM
        /// </summary>
        public event EventHandler<PreSaveEventArgs> PreSave;

        /// <summary>
        /// The event fired just after the commit of the ORM
        /// </summary>
        public event EventHandler<PostSaveEventArgs> PostSave;

        public void Dispose()
        {
            
        }
    }
}