using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoQueryBase<T> : QueryBase
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            var cmd = GetCommand(efContext);
            return cmd.CommandText;
        }

        protected IQueryable<T> PrepareQuery(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            Func<DbContext, IQueryable<T>> contextQuery = c =>
            {
                var cmd = GetCommand(c);
                return cmd.ExecuteCommandWithResults(MapReaderResults);
            };

            return contextQuery(efContext);
        }

        protected abstract IDbCommand GetCommand(DbContext c);

        protected abstract IEnumerable<T> MapReaderResults(IDataReader reader);
    }
}
