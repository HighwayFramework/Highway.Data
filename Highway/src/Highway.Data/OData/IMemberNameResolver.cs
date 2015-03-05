using System;
using System.Reflection;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Defines the public interface for a resolver of <see cref="MemberInfo" /> name.
    /// </summary>
    public interface IMemberNameResolver
    {
        /// <summary>
        ///     Returns the resolved name for the <see cref="MemberInfo" />.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo" /> to resolve the name of.</param>
        /// <returns>The resolved name.</returns>
        string ResolveName(MemberInfo member);

        /// <summary>
        ///     Returns the resolved <see cref="MemberInfo" /> for an alias.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> the alias relates to.</param>
        /// <param name="alias">The name of the alias.</param>
        /// <returns>The <see cref="MemberInfo" /> which is aliased.</returns>
        MemberInfo ResolveAlias(Type type, string alias);
    }
}