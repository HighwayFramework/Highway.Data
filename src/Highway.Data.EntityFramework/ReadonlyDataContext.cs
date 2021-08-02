using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.EntityFramework;

namespace Highway.Data
{
    public class ReadonlyDataContext : DbContext, IReadonlyEntityDataContext
    {
        private readonly bool _databaseFirst;

        private readonly ILog _log;

        private readonly IMappingConfiguration _mapping;

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        public ReadonlyDataContext(string connectionString, IMappingConfiguration mapping)
            : this(connectionString, mapping, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(string connectionString, IMappingConfiguration mapping, ILog log)
            : this(connectionString, mapping, new DefaultContextConfiguration(), log)
        {
        }

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="contextConfiguration">
        ///     The context specific configuration that will change context level behavior (
        ///     Optional )
        /// </param>
        public ReadonlyDataContext(
            string connectionString,
            IMappingConfiguration mapping,
            IContextConfiguration contextConfiguration)
            : this(connectionString, mapping, contextConfiguration, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(
            string connectionString,
            IMappingConfiguration mapping,
            IContextConfiguration contextConfiguration,
            ILog log)
            : base(connectionString)
        {
            _mapping = mapping;
            _log = log;
            Database.Log = _log.Debug;
            if (contextConfiguration != null)
            {
                contextConfiguration.ConfigureContext(this);
            }
        }

        /// <summary>
        ///     Database first way to construct the data context for Highway.Data.EntityFramework
        /// </summary>
        /// <param name="databaseFirstConnectionString">
        ///     The metadata embedded connection string from database first Entity
        ///     Framework
        /// </param>
        public ReadonlyDataContext(string databaseFirstConnectionString)
            : this(
                databaseFirstConnectionString,
                new NoOpLogger())
        {
        }

        /// <summary>
        ///     Database first way to construct the data context for Highway.Data.EntityFramework
        /// </summary>
        /// <param name="databaseFirstConnectionString">
        ///     The metadata embedded connection string from database first Entity
        ///     Framework
        /// </param>
        /// <param name="log">The logger for the database first context</param>
        public ReadonlyDataContext(string databaseFirstConnectionString, ILog log)
            : base(databaseFirstConnectionString)
        {
            _databaseFirst = true;
            _log = log;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="dbConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        public ReadonlyDataContext(DbConnection dbConnection, bool contextOwnsConnection, IMappingConfiguration mapping)
            : this(dbConnection, contextOwnsConnection, mapping, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="dbConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(
            DbConnection dbConnection,
            bool contextOwnsConnection,
            IMappingConfiguration mapping,
            ILog log)
            : this(dbConnection, contextOwnsConnection, mapping, new DefaultContextConfiguration(), log)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="dbConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
        public ReadonlyDataContext(
            DbConnection dbConnection,
            bool contextOwnsConnection,
            IMappingConfiguration mapping,
            IContextConfiguration contextConfiguration)
            : this(dbConnection, contextOwnsConnection, mapping, contextConfiguration, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="dbConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(
            DbConnection dbConnection,
            bool contextOwnsConnection,
            IMappingConfiguration mapping,
            IContextConfiguration contextConfiguration,
            ILog log)
            : base(dbConnection, contextOwnsConnection)
        {
            _mapping = mapping;
            _log = log;
            Database.Log = _log.Debug;
            if (contextConfiguration != null)
            {
                contextConfiguration.ConfigureContext(this);
            }
        }

        /// <summary>
        ///     This gives a mockable wrapper around the normal <see cref="DbSet{T}" /> method that allows for testablity
        ///     This method is now virtual to allow for the injection of cross cutting concerns.
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        public virtual IQueryable<T> AsQueryable<T>()
            where T : class
        {
            _log.DebugFormat("Querying Object {0}", typeof(T).Name);
            var result = base.Set<T>();
            _log.TraceFormat("Queried Object {0}", typeof(T).Name);

            return result;
        }

        /// <summary>
        ///     Detaches the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to detach</param>
        /// <returns>The <typeparamref name="T" /> you detached</returns>
        public virtual T Detach<T>(T item)
            where T : class
        {
            _log.TraceFormat("Retrieving State Entry For Object {0}", item);
            var entry = GetChangeTrackingEntry(item);
            _log.DebugFormat("Detaching Object {0}", item);
            if (entry == null)
            {
                throw new InvalidOperationException("Cannot detach an object that is not attached to the current context.");
            }

            entry.State = EntityState.Detached;
            _log.TraceFormat("Detached Object {0}", item);

            return item;
        }

        /// <summary>
        ///     Executes a SQL command and tries to map the returned datasets into an <see cref="IEnumerable{T}" />
        ///     The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}" /> from the query return</returns>
        public virtual IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            var parameters =
                dbParams.Select(x => string.Format("{0} : {1} : {2}\t", x.ParameterName, x.Value, x.DbType)).ToArray();

            _log.TraceFormat("Executing SQL {0}, with parameters {1}", sql, string.Join(",", parameters));

            return Database.SqlQuery<T>(sql, dbParams);
        }

        /// <summary>
        ///     Reloads the provided instance of <typeparamref name="T" /> from the database
        /// </summary>
        /// <typeparam name="T">The Entity Type being reloaded</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to reload</param>
        /// <returns>The <typeparamref name="T" /> you reloaded</returns>
        public virtual T Reload<T>(T item)
            where T : class
        {
            _log.TraceFormat("Retrieving State Entry For Object {0}", item);
            var entry = GetChangeTrackingEntry(item);
            _log.DebugFormat("Reloading Object {0}", item);
            if (entry == null)
            {
                throw new InvalidOperationException("You cannot reload an objecct that is not in the current Entity Framework datya context");
            }

            entry.Reload();
            _log.TraceFormat("Reloaded Object {0}", item);

            return item;
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(SaveChanges)}");
        }

        public override Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(SaveChangesAsync)}");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(SaveChangesAsync)}");
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(Set)}");
        }

        public override DbSet Set(Type entityType)
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(Set)}");
        }

        protected virtual DbEntityEntry<T> GetChangeTrackingEntry<T>(T item)
            where T : class
        {
            var entry = Entry(item);

            return entry;
        }

        /// <summary>
        ///     This method is called when the model for a derived context has been initialized, but
        ///     before the model has been locked down and used to initialize the context.  The default
        ///     implementation of this method takes the <see cref="IMappingConfiguration" /> array passed in on construction and
        ///     applies them.
        ///     If no configuration mappings were passed it it does nothing.
        /// </summary>
        /// <remarks>
        ///     Typically, this method is called only once when the first instance of a derived context
        ///     is created.  The model for that context is then cached and is for all further instances of
        ///     the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///     property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///     More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///     classes directly.
        /// </remarks>
        /// <param name="modelBuilder">The builder that defines the model for the context being created</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (_databaseFirst)
            {
                throw new UnintentionalCodeFirstException();
            }

            _log.Debug("\tOnModelCreating");
            if (_mapping != null)
            {
                _log.TraceFormat("\t\tMapping : {0}", _mapping.GetType().Name);
                _mapping.ConfigureModelBuilder(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(ShouldValidateEntity)}");
        }

        protected override DbEntityValidationResult ValidateEntity(
            DbEntityEntry entityEntry,
            IDictionary<object, object> items)
        {
            throw new NotImplementedException($"{nameof(ReadonlyDataContext)} does not implement {nameof(ValidateEntity)}");
        }
    }
}
