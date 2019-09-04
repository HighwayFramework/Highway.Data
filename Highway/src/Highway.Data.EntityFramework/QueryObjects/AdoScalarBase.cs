using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoScalarBase<T> : QueryBase
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        protected abstract T MapReaderResults(IDataReader reader);

        public T Execute(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            Func<DbContext, T> contextQuery = c =>
            {
                var cmd = GetCommand(c);
                return cmd.ExecuteCommandWithResult(MapReaderResults);
            };
            return contextQuery(efContext);
        }

        protected abstract IDbCommand GetCommand(DbContext c);
    }
}
