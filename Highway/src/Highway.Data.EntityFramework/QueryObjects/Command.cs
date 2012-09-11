using System;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class Command : QueryBase, ICommand
    {
        /// <summary>
        /// The Command that will be executed at some point in the future
        /// </summary>
        protected Action<IDataContext> ContextQuery { get; set; }

        #region ICommand Members

        /// <summary>
        /// Executes the expression against the passed in context and ignores the returned value if any
        /// </summary>
        /// <param name="context">The data context that the command is executed against</param>
        public virtual void Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            ContextQuery(context);
        }

        #endregion
    }
}