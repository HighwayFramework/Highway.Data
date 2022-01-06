using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Highway.Data
{
    public class SqlQuery<T> : IQuery<T>
    {
        protected Func<DbConnection, IEnumerable<T>> ContextQuery;

        public string SqlStatement { get; set; }

        public IEnumerable<T> Execute(IQueryableProvider context)
        {
            if (!(context is DbContext efContext))
            {
                throw new InvalidOperationException("You cannot execute EF Sql Queries against a non-EF context");
            }

            using (var conn = new SqlConnection(efContext.Database.Connection.ConnectionString))
            {
                return ContextQuery.Invoke(conn);
            }
        }

        public string OutputQuery(IQueryableProvider context)
        {
            return SqlStatement;
        }

        public string OutputSQLStatement(IQueryableProvider context)
        {
            return OutputQuery(context);
        }
    }
}
