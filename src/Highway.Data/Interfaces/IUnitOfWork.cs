using System.Threading.Tasks;

namespace Highway.Data
{
    /// <summary>
    ///     This is the interface that gives an API for the Create, Update, and Delete functions of CRUD
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Adds the provided instance of <typeparamref name="T" /> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being added</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to add</param>
        /// <returns>The <typeparamref name="T" /> you added</returns>
        T Add<T>(T item) where T : class;

        /// <summary>
        ///     Removes the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being removed</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to remove</param>
        /// <returns>The <typeparamref name="T" /> you removed</returns>
        T Remove<T>(T item) where T : class;

        /// <summary>
        ///     Updates the provided instance of <typeparamref name="T" /> in the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being updated</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to update</param>
        /// <returns>The <typeparamref name="T" /> you updated</returns>
        T Update<T>(T item) where T : class;


        /// <summary>
        ///     Reloads the provided instance of <typeparamref name="T" /> from the database
        /// </summary>
        /// <typeparam name="T">The Entity Type being reloaded</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to reload</param>
        /// <returns>The <typeparamref name="T" /> you reloaded</returns>
        T Reload<T>(T item) where T : class;

        /// <summary>
        ///     Commits all currently tracked entity changes
        /// </summary>
        /// <returns>the number of rows affected</returns>
        int Commit();

        /// <summary>
        ///     Commits all currently tracked entity changes asynchronously
        /// </summary>
        /// <returns>the number of rows affected</returns>
        Task<int> CommitAsync();
    }
}