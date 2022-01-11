using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.EntityFramework;

namespace Highway.Data
{
    public class ReadonlyDataContext : IReadonlyEntityDataContext
    {
        private readonly ReadonlyDbContext _context;

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
        {
            _context = new ReadonlyDbContext(connectionString, mapping, contextConfiguration, log);
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
        {
            _context = new ReadonlyDbContext(databaseFirstConnectionString, log);
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
        {
            _context = new ReadonlyDbContext(dbConnection, contextOwnsConnection, mapping, contextConfiguration, log);
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
            return _context.InnerSet<T>();
        }

        public void Dispose()
        {
            _context?.Dispose();
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
            return _context.ExecuteSqlQuery<T>(sql, dbParams);
        }
    }
}
