
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Highway.Data
{
	public class SqlQuery<T> : IQuery<T>
	{
		protected Func<DbConnection, IEnumerable<T>> ContextQuery;

		public string SqlStatement { get; set; }

		public IEnumerable<T> Execute(IReadOnlyUnitOfWork context)
		{
			var efContext = context as DbContext;
			if (efContext == null)
			{
				throw new InvalidOperationException("You cannot execute EF Sql Queries against a non-EF context");
			}
			using (var conn = new SqlConnection(efContext.Database.Connection.ConnectionString))
			{
				return ContextQuery.Invoke(conn);
			}
		}

		public string OutputQuery(IReadOnlyUnitOfWork context)
		{
			return SqlStatement;
		}

		public string OutputSQLStatement(IReadOnlyUnitOfWork context)
		{
			return OutputQuery(context);
		}
	}
}