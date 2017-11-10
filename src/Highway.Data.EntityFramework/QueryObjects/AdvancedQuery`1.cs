
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
	/// <summary>
	///     This query is tied to the implementation of EntityFramework
	/// </summary>
	public class AdvancedQuery<T> : QueryBase, IQuery<T>
	{
		/// <summary>
		///     This holds the expression that will be used to create the <see cref="IQueryable{T}" /> when executed on the context
		/// </summary>
		protected Func<UnitOfWork, IQueryable<T>> ContextQuery { get; set; }

		/// <summary>
		///     This method allows for the extension of Ordering and Grouping on the prebuild Query
		/// </summary>
		/// <returns>an <see cref="IQueryable{T}" /></returns>
		protected virtual IQueryable<T> ExtendQuery()
		{
			try
			{
				return ContextQuery((UnitOfWork)Context);
			}
			catch (Exception)
			{
				throw; //just here to catch while debugging
			}
		}

		/// <summary>
		///     Gives the ability to apend an <see cref="IQueryable" /> onto the current query
		/// </summary>
		/// <param name="query">The query containing the expressions to append</param>
		/// <returns>The combined query</returns>
		protected virtual IQueryable<T> AppendExpressions(IQueryable<T> query)
		{
			var source = query;
			foreach (var exp in ExpressionList)
			{
				var newParams = exp.Item2.ToList();
				newParams.Insert(0, source.Expression);
				source = source.Provider.CreateQuery<T>(Expression.Call(null, exp.Item1, newParams));
			}
			return source;
		}

		protected virtual IQueryable<T> PrepareQuery(IReadOnlyUnitOfWork context)
		{
			Context = context;
			CheckContextAndQuery(ContextQuery);
			var query = ExtendQuery();
			return AppendExpressions(query);
		}

		/// <summary>
		///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
		///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
		/// </summary>
		/// <param name="context">the data context that the query should be executed against</param>
		/// <returns>
		///     <see cref="IEnumerable{T}" />
		/// </returns>
		public virtual IEnumerable<T> Execute(IReadOnlyUnitOfWork context)
		{
			var task = PrepareQuery(context);
			return task;
		}

		public string OutputQuery(IReadOnlyUnitOfWork context)
		{
			var query = PrepareQuery(context);
			return query.ToString();
		}
	}
}