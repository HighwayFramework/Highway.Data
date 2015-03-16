using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Security.DataEntitlements
{
    public static class SecurityExtensions
    {
        /// <summary>
        /// Creates a new instance of <see cref="SecuredRelationshipFragment{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type to create a RelationshipFragment for.</typeparam>
        /// <param name="config">Variable not used.  Limits the scope of the extension method to instances of IMappingConfiguration.</param>
        /// <returns>A new instance of <see cref="SecuredRelationshipFragment{T}"/> of <typeparamref name="T"/>.</returns>
        public static SecuredRelationshipFragment<T> Secure<T>(this ISecuredRelationshipBuilder config) where T : class
        {
            return new SecuredRelationshipFragment<T>();
        }

        public static LambdaExpression CombineCollectionPropertySelectorWithPredicate(LambdaExpression propertySelector, LambdaExpression propertyPredicate, CollectionBehavior collectionBehavior = CollectionBehavior.All)
        {
            // x => ids.Contains(x.Id)
            // x => Collection.Any(ids.Contains(x.Id))
            var memberExpression = propertySelector.Body as MemberExpression;
            var propertyInfo = ((PropertyInfo)memberExpression.Member);
            var containingType = propertyInfo.DeclaringType;
            if (memberExpression == null)
            {
                throw new ArgumentException("propertySelector");
            }
            var rootPredicateType = typeof(Func<,>).GetGenericTypeDefinition().MakeGenericType(containingType, typeof(bool));
            MethodInfo filterMethodInfo;
            if (collectionBehavior == CollectionBehavior.All)
            {
                filterMethodInfo = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(mi => mi.Name == "All" && mi.GetParameters().Count() == 2).MakeGenericMethod(propertyInfo.PropertyType.ToSingleType());
            }
            else
            {
                filterMethodInfo = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(mi => mi.Name == "Any" && mi.GetParameters().Count() == 2).MakeGenericMethod(propertyInfo.PropertyType.ToSingleType());
            }
            var collectionCall = Expression.Call(null, filterMethodInfo, memberExpression, propertyPredicate);
            return Expression.Lambda(rootPredicateType, collectionCall, propertySelector.Parameters);
        }
    }
}
