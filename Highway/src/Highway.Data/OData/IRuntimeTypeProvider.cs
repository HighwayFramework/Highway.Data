using System;
using System.Collections.Generic;
using System.Reflection;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Provides a type matching the provided members.
    /// </summary>
    public interface IRuntimeTypeProvider
    {
        /// <summary>
        ///     Gets the <see cref="Type" /> matching the provided members.
        /// </summary>
        /// <param name="sourceType">The <see cref="Type" /> to generate the runtime type from.</param>
        /// <param name="properties">The <see cref="MemberInfo" /> to use to generate properties.</param>
        /// <returns>A <see cref="Type" /> mathing the provided properties.</returns>
        Type Get(Type sourceType, IEnumerable<MemberInfo> properties);
    }
}