using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
    public abstract class SqlScalar<T> : IScalar<T>
    {
        protected Func<SqlConnection, T> ContextQuery;

        public T Execute(IReadonlyDataContext context)
        {
            var efContext = context as DbContext;
            using (var conn = new SqlConnection(efContext.Database.Connection.ConnectionString))
            {
                return ContextQuery.Invoke(conn);
            }
        }
    }
}
