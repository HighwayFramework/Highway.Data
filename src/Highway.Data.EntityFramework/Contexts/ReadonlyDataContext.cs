using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data
{
    public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyEntityDataContext
    {
        private readonly ILog _log;

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="nameOrConnectionString">The standard SQL connection string for the Database</param>
        public ReadonlyDataContext(string nameOrConnectionString)
            : this(nameOrConnectionString, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="nameOrConnectionString">The standard SQL connection string for the Database</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(string nameOrConnectionString, ILog log)
            : this(nameOrConnectionString, new DefaultContextConfiguration(), log)
        {
        }

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="nameOrConnectionString">The standard SQL connection string for the Database</param>
        /// <param name="contextConfiguration">
        ///     The context specific configuration that will change context level behavior (
        ///     Optional )
        /// </param>
        public ReadonlyDataContext(
            string nameOrConnectionString,
            IContextConfiguration contextConfiguration)
            : this(nameOrConnectionString, contextConfiguration, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs a context
        /// </summary>
        /// <param name="nameOrConnectionString">The standard SQL connection string for the Database</param>
        /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(
            string nameOrConnectionString,
            IContextConfiguration contextConfiguration,
            ILog log)
            : base(nameOrConnectionString)
        {
            _log = log;
            Database.Log = _log.Debug;
            if (contextConfiguration != null)
            {
                contextConfiguration.ConfigureContext(this);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="existingConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        public ReadonlyDataContext(DbConnection existingConnection, bool contextOwnsConnection)
            : this(existingConnection, contextOwnsConnection, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="existingConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(
            DbConnection existingConnection,
            bool contextOwnsConnection,
            ILog log)
            : this(existingConnection, contextOwnsConnection, new DefaultContextConfiguration(), log)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="existingConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
        public ReadonlyDataContext(
            DbConnection existingConnection,
            bool contextOwnsConnection,
            IContextConfiguration contextConfiguration)
            : this(existingConnection, contextOwnsConnection, contextConfiguration, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="existingConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
        /// <param name="log">The logger being supplied for this context ( Optional )</param>
        public ReadonlyDataContext(
            DbConnection existingConnection,
            bool contextOwnsConnection,
            IContextConfiguration contextConfiguration,
            ILog log)
            : base(existingConnection, contextOwnsConnection)
        {
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
            var result = Set<T>();
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
    }
}
