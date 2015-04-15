using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.EntityFramework.Security.Expressions
{
    internal static class SecuredIdSelectBuilder
    {
        public static LambdaExpression BuildSelectorForCollection(Type rootType, Type typeInCollection, Type keyType,
            LambdaExpression pathToSecuredRelationship)
        {
            // We are building one of the below expressions
            // x => x.CollectionProperty.Select(c => c.Id)

            // Create the constructed generic type of Func<T, IEnumerable<TKey>> without having access to a generic parameter of T ( the parameter rootType is the result of typeof(T) )
            var typeForRootSelector = BuildFuncOfTAndTkey(rootType, keyType);

            // Create a parameter that represents the object from the collection, this will later become the {parameter of typeInCollection} => part of a lambda
            var parameterForSingleId = Expression.Parameter(typeInCollection);

            // Create an Expression to turn the parameter into an accessor like {parameter of typeInCollection}.Id, when combined in a lambda this will become {parameter of typeInCollection} => {parameter of typeInCollection}.Id
            var accessorForSingleId = Expression.PropertyOrField(parameterForSingleId, "Id");

            // create the selection lambda for x => x.CollectionProperty.Select({new lambda});
            var selectionLambda = Expression.Lambda(accessorForSingleId, parameterForSingleId);

            // Reflect the generic method information from the keyType for checking later. This will be rootType.CollectionProperty.Select({new lambda}) 
            var selectMethod = BuildSelectMethod(typeInCollection, keyType, 2);

            //Project the Id property off the collection
            var selectCall = Expression.Call(null, selectMethod, pathToSecuredRelationship.Body, selectionLambda);

            // return the completed lambda
            return Expression.Lambda(typeForRootSelector, selectCall, pathToSecuredRelationship.Parameters);
        }

        public static LambdaExpression BuildSelectorForSelf(Type securedType, Type keyType)
        {
            // We are building the below function
            // x => new {keyType][] { x.Id }
            // Create a parameter that represents the object, this will later become the {parameter of rootType} => part of a lambda
            var parameterForSingleId = Expression.Parameter(securedType);

            // Create Return type lambda of Func<T, IEnumerable<TKey>>
            var returnedDelegateType = BuildFuncOfTAndTkey(securedType, keyType);

            // Create an Expression to turn the parameter into an accessor like {parameter of rootType}.Id, when combined in a lambda this will become {parameter of rootType} => {parameter of rootType}.Id
            var accessorForSingleId = Expression.PropertyOrField(parameterForSingleId, "Id");

            // creates the array i.e. new [] { x.Id }
            var arracyCreate = Expression.NewArrayInit(keyType, accessorForSingleId);

            // Return the Lambda
            return Expression.Lambda(returnedDelegateType, arracyCreate, parameterForSingleId);
        }

        public static LambdaExpression BuildSelectorForSingleProperty(Type securedType, Type securedByType, Type keyType,
            LambdaExpression propertyExpression)
        {
            // We are building the below function
            // x => new {keyType][] { x.Id }
            // Create Return type lambda of Func<T, IEnumerable<TKey>>
            var returnedDelegateType = BuildFuncOfTAndTkey(securedType, keyType);

            // Create an Expression to turn the parameter into an accessor like {parameter of rootType}.Id, when combined in a lambda this will become {parameter of rootType} => {parameter of rootType}.Property.Id
            var accessorForSingleId = Expression.PropertyOrField(propertyExpression.Body, "Id");

            // creates the array i.e. new [] { x.Id }
            var arracyCreate = Expression.NewArrayInit(keyType, accessorForSingleId);

            // Return the Lambda
            return Expression.Lambda(returnedDelegateType, arracyCreate, propertyExpression.Parameters);
        }

        private static MethodInfo GetEnumerableMethod(string methodName, int parameterCount)
        {
            return
                typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .First(mi => mi.Name == methodName && mi.GetParameters().Count() == parameterCount);
        }

        private static MethodInfo BuildSelectMethod(Type typeInCollection, Type keyType, int parameterCount)
        {
            return GetEnumerableMethod("Select", parameterCount).MakeGenericMethod(typeInCollection, keyType);
        }

        private static Type BuildFuncOfTAndTkey(Type rootType, Type keyType)
        {
            var enumerableTypeOfKey = typeof (IEnumerable<>).GetGenericTypeDefinition().MakeGenericType(keyType);
            return typeof (Func<,>).GetGenericTypeDefinition().MakeGenericType(rootType, enumerableTypeOfKey);
        }
    }
}