using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoBase : QueryBase
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        protected abstract string SqlStatement { get; }

        protected DbContext GetTypedContext(IDataContext context)
        {
            var efContext = context as DbContext;
            if (efContext == null)
            {
                throw new InvalidOperationException("You cannot execute EF Sql Queries against a non-EF context");
            }

            return efContext;
        }
    }

    public abstract class AdoQuery<T> : AdoBase, IQuery<T>
    {
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
            var parameters = Parameters;
            return c.CreateSqlCommand(SqlStatement, parameters?.ToArray());
        }
    }
}
