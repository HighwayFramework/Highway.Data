// <copyright file="Scalar`1.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;

namespace Highway.Data
{
    /// <summary>
    ///     Base implementation of a query that returns a single value or object
    /// </summary>
    /// <typeparam name="T">The type of object or value being returned</typeparam>
    public class Scalar<T> : QueryBase, IScalar<T>
    {
        /// <summary>
        ///     The query to be executed later
        /// </summary>
        protected Func<IDataContext, T> ContextQuery { get; set; }

        /// <summary>
        ///     Executes the expression against the passed in context
        /// </summary>
        /// <param name="context">The data context that the scalar query is executed against</param>
        /// <returns>The instance of <typeparamref name="T" /> that the query materialized if any</returns>
        public T Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);

            return ContextQuery(context);
        }
    }
}
