using System.Collections.Generic;
using System.Data.Common;

namespace Highway.Data.EntityFramework
{
    public interface IReadonlyEntityDataContext : IReadonlyDataContext
    {
        /// <summary>
        ///     Executes a SQL command and tries to map the returned dataset into an <see cref="IEnumerable{T}" />
        ///     The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An <see cref="IEnumerable{T}" /> from the query return</returns>
        IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams);
    }
}
