using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoCommandBase : AdvancedCommand
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        public AdoCommandBase()
        {
            ContextQuery = c =>
            {
                var cmd = GetCommand(c);
                cmd.ExecuteCommand();
            };
        }

        public void Execute(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            ContextQuery(efContext);
        }

        protected abstract IDbCommand GetCommand(DbContext c);
    }
}