using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Extensions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Expressions
{
    /// <summary>
    ///     Represents the path to an included property and information to access its nested, secured properties.
    /// </summary>
    public class IncludeSecurityExpressionBuilder
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IncludeSecurityExpressionBuilder" /> class.
        ///     Initializes a new instance of the <see cref="IncludeSecurityExpressionBuilder" /> with the given include path.
        /// </summary>
        /// <param name="includedPropertyPath">
        ///     The "dot" path to the included property.
        ///     <example>
        ///         Entity.PropertyA.PropertyB
        ///     </example>
        /// </param>
        public IncludeSecurityExpressionBuilder(string includedPropertyPath)
        {
            IncludedPropertyPath = includedPropertyPath;
            IncludedPropertyDetails = new List<IncludePathMetadataFragment>();
        }

        /// <summary>
        ///     Gets the collection of PropertyAccessMetadata referred to by the IncludedPropertyPath.
        /// </summary>
        public ICollection<IncludePathMetadataFragment> IncludedPropertyDetails { get; private set; }

        /// <summary>
        ///     Gets the "dot" path to the included property.
        /// </summary>
        public string IncludedPropertyPath { get; private set; }

        /// <summary>
        ///     Returns an <see cref="Expression{T}" /> of <see cref="Func{T, TResult}" /> of <see cref="T:T" />,
        ///     <see cref="bool" /> that is used to secure an entity set by the include path represented in the
        ///     IncludedPropertyPath property.
        /// </summary>
        /// <typeparam name="T">
        ///     The <see cref="System.Type" /> to be secured.
        /// </typeparam>
        /// <param name="entitlements">
        ///     Contains information about which entities the user has access to.
        /// </param>
        /// <param name="relationshipCache">
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        public Expression<Func<T, bool>> GenerateSecurityPredicate<T>(IProvideEntitlements entitlements,
            SecuredRelationshipCache relationshipCache)
        {
            if (!IncludedPropertyDetails.Any())
            {
                // If there are no included properties, we have nothing to do.
                return null;
            }

            // We need to start at the leaf property and work our way back up, so we reverse the list.
            var reversedPropertyDetails = IncludedPropertyDetails.Reverse().ToList();
            var leafProperty = reversedPropertyDetails.First();

            var leafPropertySecuredRelationships = relationshipCache.GetSecuredRelationships(leafProperty.DeclaringType);
            if (leafPropertySecuredRelationships == null)
            {
                // If the leaf property's declaring type has no secured relationships, we have nothing to do. 
                return null;
            }

            // Find the relationship that is secured by the leaf property's type.
            var securedRelationship =
                leafPropertySecuredRelationships.SingleOrDefault(x => x.SecuredBy.Contains(leafProperty.PropertyType));
            if (securedRelationship == null || securedRelationship.ByPassSecurity<T>(entitlements))
            {
                return null;
            }
            var filter = securedRelationship.GetPredicate(entitlements);
            reversedPropertyDetails.Remove(leafProperty);

            foreach (var propertyAccessMetadata in reversedPropertyDetails)
            {
                if (propertyAccessMetadata.IsEnumerable)
                {
                    filter = GetLambdaForEnumerableProperty<T>(propertyAccessMetadata, filter);
                }
                else
                {
                    filter = GetLambdaForSimpleProperty<T>(propertyAccessMetadata, filter);
                }
            }

            return (Expression<Func<T, bool>>) filter;
        }

        /// <summary>
        ///     The get lambda for enumerable property.
        /// </summary>
        /// <param name="includePathMetadataFragment">
        ///     The include path metadata fragment.
        /// </param>
        /// <param name="filterLambda">
        ///     The filter lambda.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="LambdaExpression" />.
        /// </returns>
        private LambdaExpression GetLambdaForEnumerableProperty<T>(
            IncludePathMetadataFragment includePathMetadataFragment, LambdaExpression filterLambda)
        {
            var lambdaType =
                typeof (Func<,>).GetGenericTypeDefinition()
                    .MakeGenericType(includePathMetadataFragment.DeclaringType, includePathMetadataFragment.PropertyType);
            var memberParameter = (ParameterExpression) includePathMetadataFragment.PropertyAccessExpression.Expression;
            var propertyLambda = Expression.Lambda(lambdaType, includePathMetadataFragment.PropertyAccessExpression,
                memberParameter);
            return propertyLambda.CombineCollectionPropertySelectorWithPredicate(filterLambda);
        }

        /// <summary>
        ///     The get lambda for simple property.
        /// </summary>
        /// <param name="includePathMetadataFragment">
        ///     The include path metadata fragment.
        /// </param>
        /// <param name="filterLambda">
        ///     The filter lambda.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="LambdaExpression" />.
        /// </returns>
        private LambdaExpression GetLambdaForSimpleProperty<T>(IncludePathMetadataFragment includePathMetadataFragment,
            LambdaExpression filterLambda)
        {
            var lambdaType =
                typeof (Func<,>).GetGenericTypeDefinition()
                    .MakeGenericType(includePathMetadataFragment.DeclaringType, includePathMetadataFragment.PropertyType);
            var rootParameter = Expression.Parameter(includePathMetadataFragment.DeclaringType);
            var propertyLambda = Expression.Lambda(lambdaType, includePathMetadataFragment.PropertyAccessExpression,
                rootParameter);
            return ParameterToMemberExpressionRebinder.CombineSinglePropertySelectorWithPredicate(propertyLambda,
                filterLambda);
        }
    }
}