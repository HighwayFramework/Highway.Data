using System;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements
{
    /// <summary>
    /// Represents information needed to access a property.
    /// </summary>
    public class IncludePathMetadataFragment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncludePathMetadataFragment"/> class that has the given declaring type, property type, and property access expression.
        /// </summary>
        /// <param name="declaringType">The <see cref="System.Type"/> that contains the represented property as a member.</param>
        /// <param name="propertyType">The <see cref="System.Type"/> of the represented property.</param>
        /// <param name="propertyAccessExpression">The <see cref="System.Linq.Expressions.MemberExpression"/> used to access the represented property.</param>
        public IncludePathMetadataFragment(Type declaringType, Type propertyType, MemberExpression propertyAccessExpression)
        {
            DeclaringType = declaringType;
            IsEnumerable = propertyType.IsEnumerable();
            PropertyAccessExpression = propertyAccessExpression;
            PropertySingularType = propertyType.ToSingleType();
            PropertyType = propertyType;
        }

        /// <summary>
        /// Gets the <see cref="System.Type"/> of the represented property.
        /// </summary>
        public Type DeclaringType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the represented property is an enumerable.
        /// </summary>
        public bool IsEnumerable { get; private set; }

        /// <summary>
        /// Gets the <see cref="System.Linq.Expressions.MemberExpression"/> used to access the represented property.
        /// </summary>
        public MemberExpression PropertyAccessExpression { get; private set; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> of the represented property, or if the property's <see cref="System.Type"/> implements the <see cref="T:System.Collections.IEnumerable{T}"/> interface, returns the generic type of the represented property's <see cref="System.Type"/>.
        /// </summary>
        public Type PropertySingularType { get; private set; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> of the represented property.
        /// </summary>
        public Type PropertyType { get; private set; }
    }
}
