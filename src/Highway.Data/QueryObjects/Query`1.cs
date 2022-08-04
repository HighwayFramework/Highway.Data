using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
    /// <summary>
    ///     The base implementation for Queries that return collections
    /// </summary>
    /// <typeparam name="T">The Type being requested</typeparam>
    public class Query<T> : QueryBase, IQuery<T>
    {
        /// <summary>
        ///     This holds the expression that will be used to create the <see cref="IQueryable{T}" /> when executed on the context
        /// </summary>
        protected Func<IDataSource, IQueryable<T>> ContextQuery { get; set; }

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="dataSource">the data source that the query should be executed against</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public virtual IEnumerable<T> Execute(IDataSource dataSource)
        {
            return PrepareQuery(dataSource);
        }

        public virtual string OutputQuery(IDataSource dataSource)
        {
            var query = PrepareQuery(dataSource);

            return query.ToString();
        }

        /// <summary>
        ///     This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the
        ///     IQueryable<typeparamref name="T" /> against the data source
        /// </summary>
        /// <param name="dataSource">The data source that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        public virtual string OutputSQLStatement(IDataSource dataSource)
        {
            return OutputQuery(dataSource);
        }

        /// <summary>
        ///     Gives the ability to append an <see cref="IQueryable" /> onto the current query
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

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuilt Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{T}" /></returns>
        protected virtual IQueryable<T> ExtendQuery()
        {
            return ContextQuery(DataSource);
        }

        protected virtual IQueryable<T> PrepareQuery(IDataSource dataSource)
        {
            DataSource = dataSource;
            CheckDataSourceAndQuery(ContextQuery);
            var query = ExtendQuery();

            return AppendExpressions(query);
        }
    }
}
