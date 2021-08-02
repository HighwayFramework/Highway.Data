using System.Data.Common;

namespace Highway.Data.EntityFramework
{
    /// <summary>
    /// </summary>
    public interface IEntityDataContext : IDataContext, IReadonlyEntityDataContext
    {
        /// <summary>
        ///     Attaches the provided instance of <typeparamref name="T" /> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to attach</param>
        /// <returns>The <typeparamref name="T" /> you attached</returns>
        T Attach<T>(T item)
            where T : class;

        /// <summary>
        ///     Executes a SQL command and returns the standard int return from the query
        /// </summary>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        int ExecuteSqlCommand(string sql, params DbParameter[] dbParams);
    }
}
