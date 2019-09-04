using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureQuery<T> : AdoBase, IQuery<T>
    {
        protected abstract string StoredProcedureName { get; }

        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            var efContext = GetTypedContext(context);
            var cmd = GetCommand(efContext);
            return cmd.CommandText;
        }

        protected abstract IEnumerable<T> MapReaderResults(IDataReader reader);

        protected virtual IQueryable<T> PrepareQuery(IDataContext context)
        {
            var efContext = GetTypedContext(context);
            Func<DbContext, IQueryable<T>> contextQuery = c =>
            {
                var cmd = GetCommand(c);
                return cmd.ExecuteCommandWithResults(MapReaderResults);
            };

            return contextQuery(efContext);
        }

        private IDbCommand GetCommand(DbContext c)
        {
            return c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
        }
    }
}
