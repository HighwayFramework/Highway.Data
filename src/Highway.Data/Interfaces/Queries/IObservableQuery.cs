// <copyright file="IObservableQuery.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    /// <summary>
    ///     The interface used to surface events for queries that support interceptors
    /// </summary>
    public interface IObservableQuery
    {
        /// <summary>
        ///     The event fired just before the query goes to the database
        /// </summary>
        event EventHandler<BeforeQuery> BeforeQuery;

        /// <summary>
        ///     The event fire just after the data is translated into objects but before the data is returned.
        /// </summary>
        event EventHandler<AfterQuery> AfterQuery;
    }

    /// <summary>
    ///     An Interface for Queries that return collections
    /// </summary>
    /// <typeparam name="TSelection">The type being queried</typeparam>
    /// <typeparam name="TProjection">The type to be returned</typeparam>
    public interface IQuery<out TSelection, out TProjection> : IQuery<TProjection>
    {
    }
}
