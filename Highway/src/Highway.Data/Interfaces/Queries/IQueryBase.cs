namespace Highway.Data.Interfaces
{
    /// <summary>
    /// The base interface that surfaces SQL output of the Query statement
    /// </summary>
    public interface IQueryBase
    {
        /// <summary>
        /// This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the IQueryable against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns>The SQL Statement from the Query</returns>
        string OutputSQLStatement(IDataContext context);
    }
}