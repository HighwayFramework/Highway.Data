using System;

namespace Highway.Data
{
    /// <summary>
    ///     Advanced query that returns a single value or object base on Entity Framework
    /// </summary>
    /// <typeparam name="T">The type of object or value being returned</typeparam>
    public class AdvancedScalar<T> : QueryBase, IScalar<T>
    {
        /// <summary>
        ///     The query to be executed later
        /// </summary>
        protected Func<DataContext, T> ContextQuery { get; set; }

        /// <summary>
        ///     Executes the expression against the passed in context
        /// </summary>
        /// <param name="dataSource">The data context that the scalar query is executed against</param>
        /// <returns>The instance of <typeparamref name="T" /> that the query materialized if any</returns>
        public T Execute(IDataSource dataSource)
        {
            DataSource = dataSource;
            CheckDataSourceAndQuery(ContextQuery);

            return ContextQuery((DataContext)dataSource);
        }
    }
}
