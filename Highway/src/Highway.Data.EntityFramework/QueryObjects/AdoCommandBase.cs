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

        public void Execute(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            Action<DbContext> contextQuery = c =>
            {
                var cmd = GetCommand(c);
                cmd.ExecuteCommand();
            };
            contextQuery(efContext);
        }

        protected abstract IDbCommand GetCommand(DbContext c);
    }
}