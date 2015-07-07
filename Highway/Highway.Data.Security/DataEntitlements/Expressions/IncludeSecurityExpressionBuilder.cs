using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    /// <summary>
    /// Represents the path to an included property and information to access its nested,  properties.
    /// </summary>
    public class IncludeSecurityExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncludeSecurityExpressionBuilder"/> with the given include path.
        /// </summary>
        /// <param name="includedPropertyPath">The "dot" path to the included property.
        /// <example>Entity.PropertyA.PropertyB</example>
        /// </param>
        public IncludeSecurityExpressionBuilder(string includedPropertyPath)
        {
            IncludedPropertyPath = includedPropertyPath;
            IncludedPropertyDetails = new List<IncludePathMetadataFragment>();
        }

        /// <summary>
        /// Returns an <see cref="Expression{T}"/> of <see cref="Func{T, TResult}"/> of <see cref="T:T"/>, <see cref="bool"/> that is used to secure an entity set by the include path represented in the IncludedPropertyPath property.
        /// </summary>
        /// <typeparam name="T">The <see cref="System.Type"/> to be .</typeparam>
        /// <param name="entitlements">Contains information about which entities the user has access to.</param>
        /// <param name="mappingCache"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GenerateSecurityPredicate<T>(IEntitlementProvider entitlements, MappingsCache mappingCache)
        {
            if (!IncludedPropertyDetails.Any())
            {
                // If there are no included properties, we have nothing to do.
                return null;
            }

            // We need to start at the leaf property and work our way back up, so we reverse the list.
            var reversedPropertyDetails = IncludedPropertyDetails.Reverse().ToList();
            var leafProperty = reversedPropertyDetails.First();

            var leafPropertyRelationships = mappingCache.GetRelationships(leafProperty.DeclaringType);
            if (leafPropertyRelationships == null)
            {
                // If the leaf property's declaring type has no  relationships, we have nothing to do. 
                return null;
            }

            // Find the relationship that is  by the leaf property's type.
            var Relationship = leafPropertyRelationships.SingleOrDefault(x => x.SecuredBy == leafProperty.PropertySingularType);
            if (!RelationshipNeedsToBe<T>(entitlements, Relationship))
            {
                return null;
            }

            var entitledIds = entitlements.GetEntitledIds(Relationship.SecuredBy);
            var filterLambda = Relationship.GetSimplePredicate(entitledIds);
            reversedPropertyDetails.Remove(leafProperty);

            foreach (var propertyAccessMetadata in reversedPropertyDetails)
            {
                if (propertyAccessMetadata.IsEnumerable)
                {
                    filterLambda = GetLambdaForEnumerableProperty<T>(propertyAccessMetadata, filterLambda);
                }
                else
                {
                    filterLambda = GetLambdaForSimpleProperty<T>(propertyAccessMetadata, filterLambda);
                }
            }

            return (Expression<Func<T, bool>>)filterLambda;
        }

        private LambdaExpression GetLambdaForEnumerableProperty<T>(IncludePathMetadataFragment includePathMetadataFragment, LambdaExpression filterLambda)
        {
            var lambdaType = typeof(Func<,>).GetGenericTypeDefinition().MakeGenericType(includePathMetadataFragment.DeclaringType, includePathMetadataFragment.PropertyType);
            var memberParameter = (ParameterExpression)includePathMetadataFragment.PropertyAccessExpression.Expression;
            var propertyLambda = Expression.Lambda(lambdaType, includePathMetadataFragment.PropertyAccessExpression, memberParameter);
            return SecurityExtensions.CombineCollectionPropertySelectorWithPredicate(propertyLambda, filterLambda);
        }

        private LambdaExpression GetLambdaForSimpleProperty<T>(IncludePathMetadataFragment includePathMetadataFragment, LambdaExpression filterLambda)
        {
            var lambdaType = typeof(Func<,>).GetGenericTypeDefinition().MakeGenericType(includePathMetadataFragment.DeclaringType, includePathMetadataFragment.PropertyType);
            var rootParameter = Expression.Parameter(includePathMetadataFragment.DeclaringType);
            var propertyLambda = Expression.Lambda(lambdaType, includePathMetadataFragment.PropertyAccessExpression, rootParameter);
            return ParameterToMemberExpressionRebinder.CombineSinglePropertySelectorWithPredicate(propertyLambda, filterLambda);
        }

        private bool RelationshipNeedsToBe<T>(IEntitlementProvider entitlements, SecuredRelationship securedRelationship)
        {
            return securedRelationship != null && !entitlements.IsEntitledToAll(securedRelationship.SecuredBy);
        }

        /// <summary>
        /// Gets the collection of PropertyAccessMetadata referred to by the IncludedPropertyPath.
        /// </summary>
        public ICollection<IncludePathMetadataFragment> IncludedPropertyDetails { get; private set; }

        /// <summary>
        /// Gets the "dot" path to the included property.
        /// </summary>
        public string IncludedPropertyPath { get; private set; }
    }
}
