using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.EntityFramework;

namespace Highway.Data
{
    public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyEntityDataContext
    {
        private readonly bool _databaseFirst;

        private readonly ILog _log;

        private readonly IMappingConfiguration _mapping;

        /// <summary>
        ///     Constructs a readonly context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        public ReadonlyDataContext(string connectionString, IMappingConfiguration mapping)
            : this(connectionString, mapping, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs a readonly context
        /// </summary>
        /// <param name="connectionString">The standard SQL connection string for the Database</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(string connectionString, IMappingConfiguration mapping, ILog log)
            : this(connectionString, mapping, new DefaultContextConfiguration(), log)
        {
        }

        /// <summary>
        ///     Constructs a readonly context
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
        ///     Constructs a readonly context
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
            : this(databaseFirstConnectionString, new NoOpLogger())
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
        ///     This gives a mockable wrapper around the normal <see cref="DbSet{TEntity}" /> method that allows for testability
        ///     This method is now virtual to allow for the injection of cross cutting concerns.
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        public IQueryable<T> AsQueryable<T>()
            where T : class
        {
            _log.Debug($"Querying Object {typeof(T).Name}");
            var result = InnerSet<T>();
            _log.Trace($"Queried Object {typeof(T).Name}");

            return result;
        }

        /// <summary>
        ///     Executes a SQL command and tries to map the returned dataset into an <see cref="IEnumerable{T}" />
        ///     The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}" /> from the query return</returns>
        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            var parameters = dbParams.Select(x => $"{x.ParameterName} : {x.Value} : {x.DbType}\t").ToArray();

            _log.Trace($"Executing SQL {sql}, with parameters {string.Join(",", parameters)}");

            return Database.SqlQuery<T>(sql, dbParams);
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
        ///     property on the given ModelBuilder, but note that this can seriously degrade performance.
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
    }
}
