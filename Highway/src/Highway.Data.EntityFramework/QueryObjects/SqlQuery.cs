using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
    public class SqlQuery<T> : IQuery<T>
    {
        protected Func<DbConnection, IEnumerable<T>> ContextQuery;

        public string SqlStatement { get; set; }

        public IEnumerable<T> Execute(IDataContext context)
        {
            var efContext = context as DbContext;
            using (var conn = new SqlConnection(efContext.Database.Connection.ConnectionString))
            {
                return ContextQuery.Invoke(conn);
            }
        }

        public string OutputQuery(IDataContext context)
        {
            return SqlStatement;
        }

        public string OutputSQLStatement(IDataContext context)
        {
            return OutputQuery(context);
        }
    }
}