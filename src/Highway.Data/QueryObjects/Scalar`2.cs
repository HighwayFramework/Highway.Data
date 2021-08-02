// <copyright file="Scalar`2.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data
{
    /// <summary>
    ///     Base implementation of a query that returns a single value or object
    /// </summary>
    /// <typeparam name="TSelection">The type to query.</typeparam>
    /// <typeparam name="TProjection">The type to return.</typeparam>
    public class Scalar<TSelection, TProjection> : QueryBase, IScalar<TProjection>
        where TSelection : class
    {
        /// <summary>
        ///     the projection to take the limited result set and materialize it.
        /// </summary>
        protected Func<IQueryable<TSelection>, TProjection> Projector { get; set; }

        /// <summary>
        ///     The query to limit the result set
        /// </summary>
        protected Func<IDataContext, IQueryable<TSelection>> Selector { get; set; }

        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public virtual TProjection Execute(IDataContext context)
        {
            return PrepareQuery(context);
        }

        /// <summary>
        ///     This executes the expression against the passed in context to generate the SQL statement, but doesn't execute the
        ///     query against the data context.
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the SQL is generated against</param>
        /// <returns></returns>
        public string OutputQuery(IDataContext context)
        {
            return ExtendQuery().ToString();
        }

        /// <summary>
        ///     Gives the ability to append an <see cref="IQueryable" /> onto the current query
        /// </summary>
        /// <param name="query">The query containing the expressions to append</param>
        /// <returns>The combined query</returns>
        protected TProjection AppendExpressions(IQueryable<TSelection> query)
        {
            var source = query;
            foreach (var exp in ExpressionList)
            {
                var newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<TSelection>(Expression.Call(null, exp.Item1, newParams));
            }

            return Projector(source);
        }

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuilt Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{TSelection}" /></returns>
        protected virtual IQueryable<TSelection> ExtendQuery()
        {
            return Selector(Context);
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
