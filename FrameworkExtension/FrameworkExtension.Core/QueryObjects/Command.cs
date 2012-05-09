using System;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public class Command : QueryBase, ICommandObject
    {
        public Action<IDataContext> ContextQuery { get; set; }

        public virtual void Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            this.ContextQuery(context);
        }

    }
}