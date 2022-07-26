using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.EntityFramework;

namespace Highway.Data
{
    public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyEntityDataContext
    {
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
            : base(connectionString, mapping, contextConfiguration, log)
        {
        }

        /// <summary>
        ///     Database first way to construct the readonly data context for Highway.Data.EntityFramework
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
        ///     Database first way to construct the readonly data context for Highway.Data.EntityFramework
        /// </summary>
        /// <param name="databaseFirstConnectionString">
        ///     The metadata embedded connection string from database first Entity
        ///     Framework
        /// </param>
        /// <param name="log">The logger for the database first context</param>
        public ReadonlyDataContext(string databaseFirstConnectionString, ILog log)
            : base(databaseFirstConnectionString, log)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class.
        /// </summary>
        /// <param name="dbConnection">The db connection.</param>
        /// <param name="contextOwnsConnection">The context owns connection.</param>
        /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
        public ReadonlyDataContext(DbConnection dbConnection, bool contextOwnsConnection, IMappingConfiguration mapping)
            : this(dbConnection, contextOwnsConnection, mapping, new DefaultContextConfiguration(), new NoOpLogger())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class.
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
        ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class.
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
        ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class.
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
            : base(dbConnection, contextOwnsConnection, mapping, contextConfiguration, log)
        {
        }

        /// <summary>
        ///     This gives a mockable wrapper around the normal <see cref="DbSet{TEntity}" /> method that allows for testability
        ///     This method is now virtual to allow for the injection of cross cutting concerns.
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        public virtual IQueryable<T> AsQueryable<T>()
            where T : class
        {
            return InnerSet<T>();
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
            var entry = Entry(item);
            if (entry == null)
            {
                throw new InvalidOperationException("You cannot reload an object that is not in the current Entity Framework data context");
            }

            entry.Reload();

            return item;
        }
    }
}
