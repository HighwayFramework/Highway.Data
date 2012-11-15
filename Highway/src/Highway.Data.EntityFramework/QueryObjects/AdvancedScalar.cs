using System;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// Advanced query that returns a single value or object base on Entity Framework
    /// </summary>
    /// <typeparam name="T">The type of object or value being returned</typeparam>
    public class AdvancedScalar<T> : QueryBase, IScalar<T>
    {
        /// <summary>
        /// The query to be executed later
        /// </summary>
        protected Func<DataContext, T> ContextQuery { get; set; }

        #region IScalar<T> Members

        /// <summary>
        /// Executes the expression against the passed in context
        /// </summary>
        /// <param name="context">The data context that the scalar query is executed against</param>
        /// <returns>The instance of <typeparamref name="T"/> that the query materialized if any</returns>
        public T Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            return ContextQuery((DataContext) context);
        }

        #endregion
    }
}