using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Queries
{
    public class Scalar<T> : SecuredScalar<T, T>
        where T : class
    {
    }

    /// <summary>
    ///     Base implementation of a query that returns a single value or object
    /// </summary>
    /// <typeparam name="TSelection">
    ///     The type that is selected and passed as an IQueryable
    /// </typeparam>
    /// <typeparam name="TProjection">
    ///     The type of object or value being returned
    /// </typeparam>
    public class SecuredScalar<TSelection, TProjection> : QueryBase, IScalar<TProjection>
        where TSelection : class
    {
        /// <summary>
        ///     The query to limit the result set
        /// </summary>
        protected Func<IDataContext, IQueryable<TSelection>> Selector { get; set; }

        /// <summary>
        ///     the projection to take the limited result set and materialize it.
        /// </summary>
        protected Func<IQueryable<TSelection>, TProjection> Projector { get; set; }

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">
        ///     the data context that the query should be executed against
        /// </param>
        public virtual TProjection Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        /// <summary>
        ///     This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the
        ///     IQueryable<typeparamref name="TSelection" /> against the data context
        /// </summary>
        /// <param name="context">
        ///     The data context that the query is evaluated and the SQL is generated against
        /// </param>
        public string OutputQuery(IDataContext context)
        {
            return ExtendQuery().ToString();
        }

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuild Query
        /// </summary>
        protected virtual IQueryable<TSelection> ExtendQuery()
        {
            return Selector(Context);
        }

        /// <summary>
        ///     Gives the ability to append an <see cref="IQueryable" /> onto the current query
        /// </summary>
        /// <param name="query">
        ///     The query containing the expressions to append
        /// </param>
        /// <returns>
        ///     The combined query
        /// </returns>
        protected TProjection AppendExpressions(IQueryable<TSelection> query)
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

        private TProjection PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(Selector);
            var query = ExtendQuery();
            return AppendExpressions(query);
        }
    }
}