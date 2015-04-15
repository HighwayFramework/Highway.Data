
using System.Collections.Generic;
using System.Data.Common;


namespace Highway.Data.EntityFramework
{
    /// <summary>
    /// </summary>
    public interface IEntityDataContext : IDataContext
    {
        /// <summary>
        ///     Executes a SQL command and tries to map the returned datasets into an <see cref="IEnumerable{T}" />
        ///     The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}" /> from the query return</returns>
        IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams);

        /// <summary>
        ///     Executes a SQL command and returns the standard int return from the query
        /// </summary>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        int ExecuteSqlCommand(string sql, params DbParameter[] dbParams);

        /// <summary>
        ///     Attaches the provided instance of <typeparamref name="T" /> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to attach</param>
        /// <returns>The <typeparamref name="T" /> you attached</returns>
        T Attach<T>(T item) where T : class;

        /// <summary>
        ///     Detaches the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to detach</param>
        /// <returns>The <typeparamref name="T" /> you detached</returns>
        T Detach<T>(T item) where T : class;
    }
}