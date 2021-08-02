﻿// <copyright file="Query`2.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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
    ///     The base implementation of a query that has a projection
    /// </summary>
    /// <typeparam name="TSelection">The Type that will be selected</typeparam>
    /// <typeparam name="TProjection">The type that will be projected</typeparam>
    public class Query<TSelection, TProjection> : QueryBase, IQuery<TSelection, TProjection>
        where TSelection : class
    {
        protected Func<IQueryable<TSelection>, IQueryable<TProjection>> Projector { get; set; }

        protected Func<IDataContext, IQueryable<TSelection>> Selector { get; set; }

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
            var task = PrepareQuery(context);

            return task;
        }

        public virtual string OutputQuery(IDataContext context)
        {
            var query = PrepareQuery(context);

            return query.ToString();
        }

        /// <summary>
        ///     Gives the ability to append an <see cref="IQueryable" /> onto the current query
        /// </summary>
        /// <param name="query">The query containing the expressions to append</param>
        /// <returns>The combined query</returns>
        protected IQueryable<TProjection> AppendExpressions(IQueryable<TSelection> query)
        {
            var source = Projector(query);

            foreach (var exp in ExpressionList)
            {
                var newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<TProjection>(Expression.Call(null, exp.Item1, newParams));
            }

            return source;
        }

        /// <summary>
        ///     This method allows for the extension of Ordering and Grouping on the prebuilt Query
        /// </summary>
        /// <returns>an <see cref="IQueryable{TSelection}" /></returns>
        protected virtual IQueryable<TSelection> ExtendQuery()
        {
            return Selector(Context);
        }

        protected IQueryable<TProjection> PrepareQuery(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(Selector);
            var query = ExtendQuery();

            return AppendExpressions(query);
        }
    }
}
