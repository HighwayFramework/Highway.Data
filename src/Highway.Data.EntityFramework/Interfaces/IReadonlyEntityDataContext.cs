using System.Collections.Generic;
using System.Data.Common;

namespace Highway.Data.EntityFramework
{
    public interface IReadonlyEntityDataContext : IReadonlyDataContext
    {
        /// <summary>
        ///     Detaches the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to detach</param>
        /// <returns>The <typeparamref name="T" /> you detached</returns>
        T Detach<T>(T item)
            where T : class;

        /// <summary>
        ///     Executes a SQL command and tries to map the returned datasets into an <see cref="IEnumerable{T}" />
        ///     The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}" /> from the query return</returns>
        /// <warning>This method should not have side-effects in the database.  This is intended as a query operation, not a command operation, though it can be abused as such.  In the future it may be updated to use a readonly connection string.</warning>
        IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams);
    }
}
