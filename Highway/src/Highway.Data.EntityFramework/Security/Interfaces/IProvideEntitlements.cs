using System;
using System.Collections.Generic;

namespace Highway.Data.EntityFramework.Security.Interfaces
{
    /// <summary>
    ///     Allows callers to get entitled IDs for a given Type.
    /// </summary>
    public interface IProvideEntitlements
    {
        /// <summary>
        ///     Gets the list of entitled IDs for the given Type, T.
        /// </summary>
        /// <typeparam name="T">The secured type to get entitled IDs for.</typeparam>
        /// <returns>The list of entitled IDs for the given Type, T.</returns>
        IEnumerable<long> GetEntitledIds<T>();

        /// <summary>
        ///     Gets the list of entitled IDs for the given <paramref name="entityType" />.
        /// </summary>
        /// <param name="entityType">
        ///     The secured type to get entitled IDs for.
        /// </param>
        /// <returns>
        ///     The list of entitled IDs for the given Type, T.
        /// </returns>
        IEnumerable<long> GetEntitledIds(Type entityType);

        /// <summary>
        ///     Gets a value indicating whether all IDs are allowed for the given Type, T.
        /// </summary>
        /// <typeparam name="T">The secured type.</typeparam>
        /// <returns>A value indicating whether all IDs are allowed for the given Type, T.</returns>
        bool IsEntitledToAll<T>();

        /// <summary>
        ///     Gets a value indicating whether all IDs are entitled for the given <paramref name="entityType" />.
        /// </summary>
        /// <param name="entityType">
        ///     The secured type.
        /// </param>
        /// <returns>
        ///     A value indicating whether all IDs are entitled for the given <paramref name="entityType" />.
        /// </returns>
        bool IsEntitledToAll(Type entityType);
    }
}