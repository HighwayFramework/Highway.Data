using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
    public abstract class SqlCommand : ICommand
    {
        protected Action<SqlConnection> ContextQuery;

        public void Execute(IDataContext context)
        {
            var efContext = context as DbContext;
            using (var conn = new SqlConnection(efContext.Database.Connection.ConnectionString))
            {
                ContextQuery.Invoke(conn);
            }
        }
    }
}
