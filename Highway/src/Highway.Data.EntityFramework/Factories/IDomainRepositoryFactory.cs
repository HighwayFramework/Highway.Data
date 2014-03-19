using System;

namespace Highway.Data.Factories
{
    /// <summary>
    /// Interface for the factories of <see cref="IRepository"/>
    /// </summary>
    public interface IDomainRepositoryFactory
    {
        /// <summary>
        /// Creates a repository for the specified <see cref="IDomain"/>
        /// </summary>
        /// <typeparam name="T">Domain for repository</typeparam>
        /// <returns><see cref="IRepository"/></returns>
        IRepository Create<T>() where T : class, IDomain;

        /// <summary>
        /// Creates a repository for the specified <paramref name="type"/>
        /// </summary>
        /// <param name="type">Type for repository</param>
        /// <returns><see cref="IRepository"/></returns>
        IRepository Create(Type type);
    }
}