using System;
using Highway.Data;
using Raven.Client;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// An implementation that executes functions against the database tied to RavenDB
    /// </summary>
    public class AdvancedCommand : QueryBase, ICommand
    {
        /// <summary>
        /// The Command that will be executed at some point in the future
        /// </summary>
        protected Action<IDocumentSession> ContextQuery { get; set; }

        #region ICommand Members

        /// <summary>
        /// Executes the expression against the passed in context and ignores the returned value if any
        /// </summary>
        /// <param name="context">The data context that the command is executed against</param>
        public virtual void Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            ContextQuery((IDocumentSession)context);
        }

        #endregion
    }
}