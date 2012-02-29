using System;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public class CommandObject : ICommandObject
    {
        public Action<IDbContext> ContextQuery { get; set; }

        protected void CheckContextAndQuery(IDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (this.ContextQuery == null) throw new InvalidOperationException("Null Query cannot be executed.");
        }

        public virtual void Execute(IDbContext context)
        {
            CheckContextAndQuery(context);
            this.ContextQuery(context);
        }

    }
}