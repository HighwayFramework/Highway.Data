// <copyright file="IQuery`1.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Highway.Data
{
    /// <summary>
    ///     An Interface for Queries that return collections
    /// </summary>
    /// <typeparam name="T">The Type being requested</typeparam>
    public interface IQuery<out T> : IQueryBase
    {
        /// <summary>
        ///     This executes the expression in ContextQuery on the context that is passed in, resulting in a
        ///     <see cref="IQueryable{T}" /> that is returned as an <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="context">the data context that the query should be executed against</param>
        /// <returns>The collection of <typeparamref name="T" /> that the query materialized if any</returns>
        IEnumerable<T> Execute(IDataContext context);
    }
}
