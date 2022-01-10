using System.Linq;

namespace Highway.Data
{
    /// <summary>
    ///     Contract that provides <see cref="IQueryable" /> instances.
    /// </summary>
    public interface IDataSource
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
