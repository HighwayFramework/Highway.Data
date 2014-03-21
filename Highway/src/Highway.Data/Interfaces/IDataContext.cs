#region

using System;
using System.Linq;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     The standard interface used to interact with an ORM specific implementation
    /// </summary>
    public interface IDataContext : IUnitOfWork, IDisposable
    {
        /// <summary>
        ///     This gives a mock-able wrapper around normal Set method that allows for test-ablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        IQueryable<T> AsQueryable<T>() where T : class;
    }
}