using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using Common.Logging;
using Highway.Data.Interceptors.Events;
using Highway.Data.Interfaces;

namespace Highway.Data
{
    /// <summary>
    /// Context that is bounded to an Aggregate Root(s) for query separation
    /// </summary>
    public class AggregateDataContext : DbContext, IObservableDataContext
    {
        private readonly ILog _log;
        private readonly IMappingConfiguration[] _mappings;
        private readonly IEnumerable<Type> _typesConfigured = new List<Type>();
        private IEventManager _eventManager;

        /// <summary>
        /// Constructs the base AggregateRootContext - This should not be used
        /// </summary>
        /// <param name="configuration"></param>
        public AggregateDataContext(IAggregateConfiguration configuration) : base(configuration.ConnectionString)
        {
            _typesConfigured = configuration.TypesConfigured;
            _mappings = configuration.Mappings;
            _log = configuration.Logger;
            configuration.ContextConfiguration.ConfigureContext(this);
        }

        #region IObservableDataContext Members

        /// <summary>
        /// This gives a mock-able wrapper around the normal <see cref="DbSet{T}"/> method that allows for testablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns><see cref="IQueryable{T}"/></returns>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.DebugFormat("Querying Object {0}", typeof (T).Name);
            DbSet<T> result = Set<T>();
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
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.DebugFormat("Adding Object {0}", item);
            Set<T>().Add(item);
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
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.DebugFormat("Removing Object {0}", item);
            Set<T>().Remove(item);
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
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.TraceFormat("Retrieving State Entry For Object {0}", item);
            DbEntityEntry<T> entry = GetChangeTrackingEntry(item);
            _log.DebugFormat("Updating Object {0}", item);
            if (entry == null)
            {
                throw new InvalidOperationException(
                    "Cannot Update an object that is not attacched to the current Entity Framework data context");
            }
            entry.State = EntityState.Modified;
            _log.TraceFormat("Updated Object {0}", item);
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
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.DebugFormat("Attaching Object {0}", item);
            Set<T>().Attach(item);
            _log.TraceFormat("Attached Object {0}", item);
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
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.TraceFormat("Retrieving State Entry For Object {0}", item);
            DbEntityEntry<T> entry = GetChangeTrackingEntry(item);
            _log.DebugFormat("Detaching Object {0}", item);
            if (entry == null)
            {
                throw new InvalidOperationException(
                    "Cannot detach an object that is not attached to the current context.");
            }
            entry.State = EntityState.Detached;
            _log.TraceFormat("Detached Object {0}", item);
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
            CheckTypeAgainstAggregateRootsConfigured<T>();
            _log.TraceFormat("Retrieving State Entry For Object {0}", item);
            DbEntityEntry<T> entry = GetChangeTrackingEntry(item);
            _log.DebugFormat("Reloading Object {0}", item);
            if (entry == null)
            {
                throw new InvalidOperationException(
                    "You cannot reload an objecct that is not in the current Entity Framework datya context");
            }
            entry.Reload();
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
            base.ChangeTracker.DetectChanges();
            InvokePreSave();
            int result = base.SaveChanges();
            InvokePostSave();
            _log.DebugFormat("\tCommitted {0} Changes", result);
            return result;
        }

        /// <summary>
        /// Executes a SQL command and tries to map the returned datasets into an <see cref="IEnumerable{T}"/>
        /// The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The SQL Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}"/> from the query return</returns>
        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            string[] parameters =
                dbParams.Select(x => string.Format("{0} : {1} : {2}\t", x.ParameterName, x.Value, x.DbType)).ToArray();
            _log.TraceFormat("Executing SQL {0}, with parameters {1}", sql, string.Join(",", parameters));
            return base.Database.SqlQuery<T>(sql, dbParams);
        }

        /// <summary>
        /// Executes a SQL command and returns the standard integer return from the query
        /// </summary>
        /// <param name="sql">The SQL Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            string[] parameters =
                dbParams.Select(x => string.Format("{0} : {1} : {2}\t", x.ParameterName, x.Value, x.DbType)).ToArray();
            _log.TraceFormat("Executing SQL {0}, with parameters {1}", sql, string.Join(",", parameters));
            return base.Database.ExecuteSqlCommand(sql, dbParams);
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

        #endregion

        private DbEntityEntry<T> GetChangeTrackingEntry<T>(T item) where T : class
        {
            DbEntityEntry<T> entry = base.Entry(item);
            return entry;
        }

        private void InvokePostSave()
        {
            if (PostSave != null) PostSave(this, new PostSaveEventArgs());
        }

        private void InvokePreSave()
        {
            if (PreSave != null) PreSave(this, new PreSaveEventArgs {});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        public int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams)
        {
            string[] parameters =
                dbParams.Select(x => string.Format("{0} : {1} : {2}\t", x.Name, x.Value, x.ParameterType)).ToArray();
            _log.TraceFormat("Executing Procedure {0}, with parameters {1}", procedureName, string.Join(",", parameters));
            return base.Database.SqlQuery<int>(procedureName, dbParams).FirstOrDefault();
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        ///                 before the model has been locked down and used to initialize the context.  The default
        ///                 implementation of this method takes the <see cref="IMappingConfiguration"/> array passed in on construction and applies them. 
        /// If no configuration mappings were passed in it does nothing.
        /// </summary>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        ///                 is created.  The model for that context is then cached and is for all further instances of
        ///                 the context in the application domain.  This caching can be disabled by setting the ModelCaching
        ///                 property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///                 More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///                 classes directly.
        /// </remarks>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _log.Debug("\tOnModelCreating");
            foreach (IMappingConfiguration mapping in _mappings)
            {
                if (mapping != null)
                {
                    _log.TraceFormat("\t\tMapping : {0}", mapping.GetType().Name);
                    mapping.ConfigureModelBuilder(modelBuilder);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        private void CheckTypeAgainstAggregateRootsConfigured<T>()
        {
            if (_typesConfigured.Contains(typeof (T))) return;
            throw new InvalidOperationException(
                "Requested Type was not configured for this Aggregate Root bounded context, Please check the configuration and try again.");
        }
    }
}