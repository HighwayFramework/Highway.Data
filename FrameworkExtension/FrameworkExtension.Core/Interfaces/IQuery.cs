using System.Collections.Generic;

namespace FrameworkExtension.Core.Interfaces
{
    public interface ICommandObject
    {
        void Execute(IDataContext context);
    }

    public interface IScalarObject<out T>
    {
        T Execute(IDataContext context);
    }

    public interface IQuery<out T>
    {
        /// <summary>
        /// This executes the expression in ContextQuery on the context that is passed in, resulting in a IQueryable<typeparam name="T"></typeparam> that is returned as an IEnumerable<typeparam name="T"></typeparam>
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>IEnumerable<typeparam name="T"></typeparam></returns>
        IEnumerable<T> Execute(IDataContext context);

        /// <summary>
        /// This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the IQueryable<typeparam name="T"></typeparam> against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        string OutputSQLStatement(IDataContext context);
    }
}
