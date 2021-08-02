using System;
using System.Data.Entity;
using System.Data.SqlClient;

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
