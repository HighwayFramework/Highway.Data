using System;
using System.Linq;

namespace Highway.Data
{
    public interface IReadonlyDataContext : IDisposable, IUnitOfWork
    {
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
