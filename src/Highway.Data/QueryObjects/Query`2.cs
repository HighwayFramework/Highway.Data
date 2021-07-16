using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
    /// <summary>
    ///     The base implemetation of a query that has a projection
    /// </summary>
    /// <typeparam name="TSelection">The Type that will be selected</typeparam>
    /// <typeparam name="TProjection">The type that will be projected</typeparam>
    public class Query<TSelection, TProjection> : QueryBase, IQuery<TSelection, TProjection> 
        where TSelection : class
    {
        protected Func<IReadonlyDataContext, IQueryable<TSelection>> Selector { get; set; }
        protected Func<IQueryable<TSelection>, IQueryable<TProjection>> Projector { get; set; }

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public virtual IEnumerable<TProjection> Execute(IReadonlyDataContext context)
        {
            IQueryable<TProjection> task = PrepareQuery(context);
            return task;
        }

        public virtual string OutputQuery(IReadonlyDataContext context)
        {
            IQueryable<TProjection> query = PrepareQuery(context);

            return query.ToString();
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
                List<Expression> newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<TSelection>(Expression.Call(null, exp.Item1, newParams));
            }
            return Projector(source);
        }

        protected IQueryable<TProjection> PrepareQuery(IReadonlyDataContext context)
        {
            Context = context;
            CheckContextAndQuery(Selector);
            var query = ExtendQuery();
            return AppendExpressions(query);
        }
    }
}