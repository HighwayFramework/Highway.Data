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
        public Func<DbContext, IQueryable<T>> ContextQuery { get; }

        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        public AdoQueryBase()
        {
            ContextQuery = c =>
            {
                var cmd = GetCommand(c);
                return cmd.ExecuteCommandWithResults(MapReaderResults);
            };
        }

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

        protected virtual IQueryable<T> PrepareQuery(IDataContext context)
        {
            var efContext = context.GetTypedContext();
            return ContextQuery(efContext);
        }

        protected abstract IDbCommand GetCommand(DbContext c);

        protected abstract IEnumerable<T> MapReaderResults(IDataReader reader);
    }
}
