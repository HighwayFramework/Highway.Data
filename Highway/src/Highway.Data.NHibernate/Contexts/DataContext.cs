#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     A base implementation of the DataContext for use around a NHibernate Session
    /// </summary>
    public partial class DataContext : IDataContext, IDisposable
    {
        private readonly ILog _log;
        private readonly ISession _session;

        /// <summary>
        ///     Constructs the context
        /// </summary>
        /// <param name="session">nHibernate's session</param>
        public DataContext(ISession session)
            : this(session, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs the context
        /// </summary>
        /// <param name="session">nHibernate's session</param>
        /// <param name="log">Common Logging logger</param>
        public DataContext(ISession session, ILog log)
        {
            _session = session;
            _log = log;
        }

        /// <summary>
        ///     This gives a mockable wrapper around the normal <see cref="DbSet{T}" /> method that allows for testablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            IQueryable<T> result = null;
            _log.DebugFormat("Querying Object {0}", typeof (T).Name);
            result = _session.Query<T>();
            _log.DebugFormat("Queried Object {0}", typeof (T).Name);
            return result;
        }

        /// <summary>
        ///     Adds the provided instance of <typeparamref name="T" /> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being added</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to add</param>
        /// <returns>The <typeparamref name="T" /> you added</returns>
        public T Add<T>(T item) where T : class
        {
            _log.DebugFormat("Adding Object {0}", item);
            _session.SaveOrUpdate(item);
            _log.TraceFormat("Added Object {0}", item);
            return item;
        }

        /// <summary>
        ///     Removes the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being removed</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to remove</param>
        /// <returns>The <typeparamref name="T" /> you removed</returns>
        public T Remove<T>(T item) where T : class
        {
            _log.DebugFormat("Removing Object {0}", item);
            _session.Delete(item);
            _log.TraceFormat("Removed Object {0}", item);
            return item;
        }

        /// <summary>
        ///     Updates the provided instance of <typeparamref name="T" /> in the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being updated</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to update</param>
        /// <returns>The <typeparamref name="T" /> you updated</returns>
        public T Update<T>(T item) where T : class
        {
            _log.DebugFormat("Updating Object {0}", item);
            _session.Update(item);
            _log.TraceFormat("Updated Object {0}", item);
            return item;
        }


        /// <summary>
        ///     Reloads the provided instance of <typeparamref name="T" /> from the database
        /// </summary>
        /// <typeparam name="T">The Entity Type being reloaded</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to reload</param>
        /// <returns>The <typeparamref name="T" /> you reloaded</returns>
        public T Reload<T>(T item) where T : class
        {
            _log.DebugFormat("Reloading Object {0}", item);
            _session.Refresh(item);
            _log.TraceFormat("Reloaded Object {0}", item);
            return item;
        }

        /// <summary>
        ///     Commits all currently tracked entity changes
        /// </summary>
        /// <returns>the number of rows affected</returns>
        public int Commit()
        {
            _log.Trace("\tCommit");
            _session.Flush();
            _log.DebugFormat("\tCommited Changes");
            return 0;
        }

        public void Dispose()
        {
            _session.Close();
            _session.Dispose();
        }

        /// <summary>
        ///     Attaches the provided instance of <typeparamref name="T" /> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to attach</param>
        /// <returns>The <typeparamref name="T" /> you attached</returns>
        public T Attach<T>(T item) where T : class
        {
            _log.DebugFormat("Attaching Object {0}", item);
            _session.Persist(item);
            _log.TraceFormat("Attached Object {0}", item);
            return item;
        }

        /// <summary>
        ///     Detaches the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to detach</param>
        /// <returns>The <typeparamref name="T" /> you detached</returns>
        public T Detach<T>(T item) where T : class
        {
            _log.DebugFormat("Detaching Object {0}", item);
            _session.Evict(item);
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
        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            var executeSqlQuery = _session.CreateSQLQuery(sql);
            foreach (var dbParameter in dbParams)
            {
                executeSqlQuery.SetParameter(dbParameter.ParameterName, dbParameter.Value);
            }
            return executeSqlQuery.SetResultTransformer(Transformers.AliasToBean<T>()).List<T>();
        }

        /// <summary>
        ///     Executes a SQL command and returns the standard int return from the query
        /// </summary>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            using (var tx = _session.BeginTransaction())
            {
                IDbCommand command = _session.Connection.CreateCommand();
                tx.Enlist(command);
                command.CommandText = sql;
                foreach (var dbParameter in dbParams)
                {
                    command.Parameters.Add(dbParameter);
                }
                var output = command.ExecuteNonQuery();
                tx.Commit();
                return output;
            }
        }
    }
}