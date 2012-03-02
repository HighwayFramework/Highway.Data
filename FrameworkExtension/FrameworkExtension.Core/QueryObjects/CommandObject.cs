using System;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public class CommandObject : ICommandObject
    {
        public Action<IDataContext> ContextQuery { get; set; }

        protected void CheckContextAndQuery(IDataContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (this.ContextQuery == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }

        public virtual void Execute(IDataContext context)
        {
            CheckContextAndQuery(context);
            this.ContextQuery(context);
        }

    }
}