using System;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class Command : QueryBase, ICommandObject
    {
        /// <summary>
        /// The Command that will be executed at some point in the future
        /// </summary>
        protected Action<IDataContext> ContextQuery { get; set; }

        public virtual void Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            this.ContextQuery(context);
        }

    }
}