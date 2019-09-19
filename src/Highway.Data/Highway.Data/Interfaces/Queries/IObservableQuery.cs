using Highway.Data.Interceptors.Events;
using System;

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
    public interface IQuery<out TSelection, out TProjection> : IQuery<TProjection> { }
}