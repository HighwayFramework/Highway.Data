// <copyright file="IQueryBase.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

namespace Highway.Data
{
    /// <summary>
    ///     The base interface that surfaces SQL output of the Query statement
    /// </summary>
    public interface IQueryBase
    {
        /// <summary>
        ///     This executes the expression against the passed in context to generate the query details, but doesn't execute the
        ///     IQueryable against the data context
        /// </summary>
        /// <param name="context">The data context that the query is evaluated and the details are generated against</param>
        /// <returns>The details of the Statement from the Query</returns>
        string OutputQuery(IDataContext context);
    }
}
