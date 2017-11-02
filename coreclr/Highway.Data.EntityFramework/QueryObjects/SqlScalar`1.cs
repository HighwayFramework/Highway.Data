
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Highway.Data
{
	public abstract class SqlScalar<T> : IScalar<T>
	{
		protected Func<SqlConnection, T> ContextQuery;

		public T Execute(IDataContext context)
		{
			var efContext = context as DbContext;
			using (var conn = new SqlConnection(efContext.Database.Connection.ConnectionString))
			{
				return ContextQuery.Invoke(conn);
			}
		}
	}
}