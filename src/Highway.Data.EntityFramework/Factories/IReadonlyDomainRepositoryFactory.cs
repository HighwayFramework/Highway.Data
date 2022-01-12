using System;

namespace Highway.Data.Factories
{
    /// <summary>
    ///     Interface for the factories of <see cref="IReadonlyRepository" />
    /// </summary>
    public interface IReadonlyDomainRepositoryFactory
    {
        /// <summary>
        ///     Creates a readonly repository for the specified <see cref="IDomain" />
        /// </summary>
        /// <typeparam name="T">Domain for readonly repository</typeparam>
        /// <returns>
        ///     <see cref="IReadonlyRepository" />
        /// </returns>
        IReadonlyRepository CreateReadonly<T>()
            where T : class, IDomain;

        /// <summary>
        ///     Creates a readonly repository for the specified <paramref name="type" />
        /// </summary>
        /// <param name="type">Type for readonly repository</param>
        /// <returns>
        ///     <see cref="IReadonlyRepository" />
        /// </returns>
        IReadonlyRepository CreateReadonly(Type type);
    }
}
