using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Resolves the name or alias for a type member based on the serialization attribute.
    /// </summary>
    public class MemberNameResolver : IMemberNameResolver
    {
        private static readonly ConcurrentDictionary<MemberInfo, string> KnownMemberNames =
            new ConcurrentDictionary<MemberInfo, string>();

        private static readonly ConcurrentDictionary<string, MemberInfo> KnownAliasNames =
            new ConcurrentDictionary<string, MemberInfo>();

        /// <summary>
        ///     Returns the resolved <see cref="MemberInfo" /> for an alias.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> the alias relates to.</param>
        /// <param name="alias">The name of the alias.</param>
        /// <returns>The <see cref="MemberInfo" /> which is aliased.</returns>
        public MemberInfo ResolveAlias(Type type, string alias)
        {
            var key = type.AssemblyQualifiedName + alias;
            return KnownAliasNames.GetOrAdd(key, s => ResolveAliasInternal(type, alias));
        }

        /// <summary>
        ///     Returns the resolved name for the <see cref="MemberInfo" />.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo" /> to resolve the name of.</param>
        /// <returns>The resolved name.</returns>
        public string ResolveName(MemberInfo member)
        {
            return KnownMemberNames.GetOrAdd(member, ResolveNameInternal);
        }

        private static MemberInfo ResolveAliasInternal(Type type, string alias)
        {
            return GetMembers(type).Select(x =>
            {
                if (HasAliasAttribute(alias, x))
                {
                    return x.MemberType == MemberTypes.Field ? CheckFrontingProperty(x) : x;
                }

                if (x.Name == alias)
                {
                    return x;
                }

                return null;
            }).FirstOrDefault(x => x != null);
        }

        private static bool HasAliasAttribute(string alias, MemberInfo member)
        {
            var attributes = member.GetCustomAttributes(true);
            var dataMember = attributes.OfType<DataMemberAttribute>()
                .FirstOrDefault();
            if (dataMember != null && dataMember.Name == alias)
            {
                return true;
            }

            var xmlElement = attributes.OfType<XmlElementAttribute>()
                .FirstOrDefault();
            if (xmlElement != null && xmlElement.ElementName == alias)
            {
                return true;
            }

            var xmlAttribute = attributes.OfType<XmlAttributeAttribute>()
                .FirstOrDefault();
            if (xmlAttribute != null && xmlAttribute.AttributeName == alias)
            {
                return true;
            }
            return false;
        }

        private static MemberInfo CheckFrontingProperty(MemberInfo field)
        {
            var declaringType = field.DeclaringType;
            var correspondingProperty = declaringType.GetProperties()
                .FirstOrDefault(
                    x =>
                        string.Equals(x.Name, field.Name.Replace("_", string.Empty), StringComparison.OrdinalIgnoreCase));

            return correspondingProperty ?? field;
        }

        private static IEnumerable<MemberInfo> GetMembers(Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<MemberInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces()
                        .Where(x => !considered.Contains(x)))
                    {
                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetMembers(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return
                type.GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Instance);
        }

        private static string ResolveNameInternal(MemberInfo member)
        {
            var dataMember = member.GetCustomAttributes(typeof (DataMemberAttribute), true)
                .OfType<DataMemberAttribute>()
                .FirstOrDefault();

            if (dataMember != null && dataMember.Name != null)
            {
                return dataMember.Name;
            }

            var xmlElement = member.GetCustomAttributes(typeof (XmlElementAttribute), true)
                .OfType<XmlElementAttribute>()
                .FirstOrDefault();

            if (xmlElement != null && xmlElement.ElementName != null)
            {
                return xmlElement.ElementName;
            }

            var xmlAttribute = member.GetCustomAttributes(typeof (XmlAttributeAttribute), true)
                .OfType<XmlAttributeAttribute>()
                .FirstOrDefault();

            if (xmlAttribute != null && xmlAttribute.AttributeName != null)
            {
                return xmlAttribute.AttributeName;
            }

            return member.Name;
        }
    }
}