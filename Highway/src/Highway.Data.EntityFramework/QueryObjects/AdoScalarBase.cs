using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalarBase<T> : QueryBase
    {
        public Func<DbContext, T> ContextQuery { get; }

        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        public AdoScalarBase()
        {
            ContextQuery = c =>
            {
                var cmd = GetCommand(c);
                return cmd.ExecuteCommandWithResult(MapReaderResults);
            };
        }

        protected abstract T MapReaderResults(IDataReader reader);

        public T Execute(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            return ContextQuery(efContext);
        }

        protected abstract IDbCommand GetCommand(DbContext c);
    }
}
