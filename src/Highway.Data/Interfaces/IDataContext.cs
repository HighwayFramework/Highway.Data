using System;
using System.Linq;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    /// <summary>
    ///     The standard interface used to interact with an ORM specific implementation
    /// </summary>
    public interface IDataContext : IReadonlyDataContext
    {
        /// <summary>
        ///     The event fired just before the commit of the persistence
        /// </summary>
        event EventHandler<BeforeSave> BeforeSave;

        /// <summary>
        ///     The event fired just after the commit of the persistence
        /// </summary>
        event EventHandler<AfterSave> AfterSave;

        /// <summary>
        ///     This gives a mock-able wrapper around normal Set method that allows for testability
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        IQueryable<T> AsQueryable<T>()
            where T : class;
    }
}
