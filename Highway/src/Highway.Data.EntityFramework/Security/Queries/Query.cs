using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Queries
{
    public class Query<T> : QueryBase, IQuery<T>
    where T : class
    {
        protected Func<IDataContext, IQueryable<T>> ContextQuery { get; set; }

        public virtual IEnumerable<T> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            return PrepareQuery(context).ToString();
        }

        public string OutputSQLStatement(IDataContext context)
        {
            return OutputQuery(context);
        }

        protected IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            var queryable = query;
            foreach (var tuple in ExpressionList)
            {
                var list = tuple.Item2.ToList();
                list.Insert(0, queryable.Expression);
                queryable = queryable.Provider.CreateQuery<T>(Expression.Call(null, tuple.Item1, list));
            }
            queryable = AppendSecurity(queryable);
            return queryable;
        }

        protected virtual IQueryable<T> ExtendQuery()
        {
            return ContextQuery(Context);
        }

        private IQueryable<T> AppendSecurity(IQueryable<T> queryable)
        {
            var typedContext = Context as ISecuredDataContext;
            if (typedContext == null)
            {
                return queryable;
            }
            var expression = queryable.Expression;
            var estreamSecurityExpressionVistor = new EstreamSecurityExpressionVistor();
            estreamSecurityExpressionVistor.Visit(expression);
            var metadatas = estreamSecurityExpressionVistor.Metadatas;
            foreach (var metadata in metadatas)
            {
                var predicate = metadata.GenerateSecurityPredicate<T>(typedContext.EntitlementProvider,
                    typedContext.SecuredRelationshipCache);
                if (predicate != null)
                {
                    queryable = queryable.Where(predicate);
                }
            }
            return queryable;
        }

        private IQueryable<T> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            return AppendExpressions(ExtendQuery());
        }
    }

    public class Query<TSelection, TProjection> : QueryBase, IQuery<TSelection, TProjection>
        where TSelection : class
    {
        protected Func<IDataContext, IQueryable<TSelection>> Selector { get; set; }
        protected Func<IQueryable<TSelection>, IQueryable<TProjection>> Projector { get; set; }

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public virtual IEnumerable<TProjection> Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        public string OutputQuery(IDataContext context)
        {
            var query = PrepareQuery(context);

            return query.ToString();
        }

        /// <summary>
        ///     This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the
        ///     query against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        public string OutputSQLStatement(IDataContext context)
        {
            return OutputQuery(context);
        }

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuild Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{TSelection}" /></returns>
        protected virtual IQueryable<TSelection> ExtendQuery()
        {
            return Selector(Context);
        }

        /// <summary>
        ///     Gives the ability to apend an <see cref="IQueryable" /> onto the current query
        /// </summary>
        /// <param name="query">The query containing the expressions to append</param>
        /// <returns>The combined query</returns>
        protected IQueryable<TProjection> AppendExpressions(IQueryable<TSelection> query)
        {
            var source = query;
            foreach (var exp in ExpressionList)
            {
                var newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<TSelection>(Expression.Call(null, exp.Item1, newParams));
            }
            source = AppendSecurity(source);
            return Projector(source);
        }

        private IQueryable<TSelection> AppendSecurity(IQueryable<TSelection> queryable)
        {
            var typedContext = Context as ISecuredDataContext;
            if (typedContext == null)
            {
                return queryable;
            }
            var expression = queryable.Expression;
            var estreamSecurityExpressionVistor = new EstreamSecurityExpressionVistor();
            estreamSecurityExpressionVistor.Visit(expression);
            var metadatas = estreamSecurityExpressionVistor.Metadatas;
            foreach (var metadata in metadatas)
            {
                var predicate = metadata.GenerateSecurityPredicate<TSelection>(typedContext.EntitlementProvider,
                    typedContext.SecuredRelationshipCache);
                if (predicate != null)
                {
                    queryable = queryable.Where(predicate);
                }
            }
            return queryable;
        }

        private IQueryable<TProjection> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(Selector);
            return AppendExpressions(ExtendQuery());
        }
    }
}