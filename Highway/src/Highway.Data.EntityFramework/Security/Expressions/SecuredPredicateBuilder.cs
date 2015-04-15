using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.EntityFramework.Security.Expressions
{
    /// <summary>
    ///     This class uses expressions and reflection to build predicate factories based on security and secured mappings.
    ///     If you want to modify this class you need to be proficient in:
    ///     Reading and understanding LINQ
    ///     Reading, Modifying, and creating Expressions
    ///     Understanding closures and delegates
    ///     Understanding the Factory pattern and deferred execution
    ///     If you do not hold these skills, do not modify this code.
    /// </summary>
    public static class SecuredPredicateBuilder
    {
        public static LambdaExpression BuildPredicateFactoryForCollectionProperty(Type rootType, Type keyType,
            LambdaExpression pathToSecuredRelationship, WhenNull whenNull)
        {
            // We are building one of the below expression factories
            // ids => return x => !x.CollectionProperty.Any() || x.CollectionProperty.Any(c => ids.Contains(c.Id))
            // ids => return x => x.CollectionProperty.Any() && x.CollectionProperty.Any(c => ids.Contains(c.Id))
            // Create the constructed generic type of Func<T, bool> without having access to a generic parameter of T ( the parameter rootType is the result of typeof(T) )
            var typeInCollection = pathToSecuredRelationship.ReturnType.ToSingleType();
            var typeForRootPredicate = BuildFuncOfTBool(rootType);

            // Create a parameter that represents the object from the collection, this will later become the {parameter of typeInCollection} => part of a lambda
            var parameterForSingleId = Expression.Parameter(pathToSecuredRelationship.ReturnType.ToSingleType());

            // Create an Expression to turn the parameter into an accessor like {parameter of typeInCollection}.Id, when combined in a lambda this will become {parameter of typeInCollection} => {parameter of typeInCollection}.Id
            Expression accessorForSingleId;
            if (pathToSecuredRelationship.ReturnType.ToSingleType().IsValueType)
            {
                accessorForSingleId = parameterForSingleId;
            }
            else
            {
                accessorForSingleId = Expression.PropertyOrField(parameterForSingleId, "Id");
            }

            // Reflect the generic method information from the keyType for checking later. This will be IEnumberable<keyType>.Contains({parameter of keyType}) 
            var containsMethod = BuildContainsMethod(keyType, 2);

            // Create a parameter that represents the type of list of secured Ids that your user is entitled to access, this is IEnumerable<keyType>
            var typeForEntitledIdsParameter = BuildEnumerableOfT(keyType);

            // Create a parameter that represents the list of secured Ids that your user is entitled to access, created from the above type
            var parameterForEntitledIds = BuildIdsParameter(typeForEntitledIdsParameter);

            // Reflect LINQ methods to get the .Any() and .Any(x => [lambda predicate here]) methods
            var reflectedMethodInfoForLinqAnyThatDoesNotTakeParameters = BuildAnyMethod(typeInCollection, 1);
            var reflectedMethodInfoForLinqAnyThatTakesAPredicate = BuildAnyMethod(typeInCollection, 2);

            // Create an expression that given a collection of Ids calls contains, and passes the Id accessor from above. Outputs - ids.Contains({parameter of typeInCollection}.Id)
            var containsMethodCall = Expression.Call(null, containsMethod, parameterForEntitledIds, accessorForSingleId);

            // Create a lambda from the above method call that closures in a reference to the type in collection. Outputs - {parameter of typeInCollection} => ids.Contains({parameter of typeInCollection}.Id)
            var containsPredicate = Expression.Lambda(containsMethodCall, parameterForSingleId);

            // Create a placeholder for variations of which where predicate we are going to need based on null behavior.
            Expression wherePredicate;
            switch (whenNull)
            {
                case WhenNull.Allow:
                {
                    // When we find a null object, the security expression should treat this the same as finding an object the user has permissions to
                    // Build the follow predicate x => !x.CollectionProperty.Any() || x.CollectionProperty.Any(c => ids.Contains(c.Id))
                    wherePredicate = BuildAllowWhenNullWherePredicate(pathToSecuredRelationship, typeForRootPredicate,
                        reflectedMethodInfoForLinqAnyThatTakesAPredicate,
                        reflectedMethodInfoForLinqAnyThatDoesNotTakeParameters, containsPredicate);
                }
                    break;
                case WhenNull.Deny:
                {
                    // When we find a null object, the security expression should treat this the same as finding an object that the user doesn't have permission to
                    // Build the following predicate x => x.CollectionProperty.Any() && x.CollectionProperty.Any(c => ids.Contains(c.Id))
                    wherePredicate = BuildDenyWhenNullWherePredicate(pathToSecuredRelationship, typeForRootPredicate,
                        reflectedMethodInfoForLinqAnyThatDoesNotTakeParameters,
                        reflectedMethodInfoForLinqAnyThatTakesAPredicate, containsPredicate);
                }
                    break;
                default:
                {
                    throw new NotImplementedException(
                        "Allow and Deny are the only supported behaviors.  Stop modifying the enum without fixing the expression generator too!!");
                }
            }

            // Return a expression that when compiled will build a where predicate on execution. ids => { predicate that takes ids from above }
            return Expression.Lambda(Expression.Quote(wherePredicate), parameterForEntitledIds);
        }

        public static LambdaExpression BuildPredicateFactoryForCollectionProperty<T, TK, TId>(
            Expression<Func<T, IEnumerable<TK>>> pathToSecuredRelationship, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            return BuildPredicateFactoryForCollectionProperty(typeof (T), typeof (TId), pathToSecuredRelationship,
                whenNull);
        }

        public static LambdaExpression BuildPredicateFactoryForSelf(Type rootType, Type keyType)
        {
            // We are building one of the below expressions
            // ids => return x => ids.Contains(x.Id)
            // Create a parameter that represents the object, this will later become the {parameter of rootType} => part of a lambda
            var parameterForSingleId = Expression.Parameter(rootType);

            // Create an Expression to turn the parameter into an accessor like {parameter of rootType}.Id, when combined in a lambda this will become {parameter of rootType} => {parameter of rootType}.Id
            var accessorForSingleId = Expression.PropertyOrField(parameterForSingleId, "Id");

            // Create a parameter that represents the type of list of secured Ids that your user is entitled to access, this is IEnumerable<keyType>
            var typeForEntitledIdsParameter = BuildEnumerableOfT(keyType);

            // Create a parameter that represents the of list of secured Ids that your user is entitled to access, created from the above type
            var parameterForEntitledIds = BuildIdsParameter(typeForEntitledIdsParameter);

            // Reflect the generic method infromation from the keyType for checking later. This will be IEnumberable<keyType>.Contains({parameter of keyType}) 
            var containsMethod = BuildContainsMethod(keyType, 2);

            // Create an expression that given a collection of Ids calls contains, and passes the Id accessor from above. Outputs - ids.Contains({parameter of rootType}.Id)
            var containsMethodCall = Expression.Call(null, containsMethod, parameterForEntitledIds, accessorForSingleId);

            // Build a Func<IEnumerable<keyType>, bool> that will be used to wrap the Contains expression, so we can closure the reference to the Ids collection provided.
            // This will produce the x => ids.Contains(x.Id) lambda type.
            var containsLambdaType = BuildFuncOfTBool(rootType);

            // build the actual lambda, providing the Contains method call expressions
            var containsMethodLambda = Expression.Lambda(containsLambdaType, containsMethodCall, parameterForSingleId);

            // Create a lambda from the above method call that closures in a reference to the type.
            return Expression.Lambda(Expression.Quote(containsMethodLambda), parameterForEntitledIds);
        }

        public static LambdaExpression BuildPredicateFactoryForSelf<T, TId>()
        {
            return BuildPredicateFactoryForSelf(typeof (T), typeof (TId));
        }

        public static LambdaExpression BuildPredicateFactoryForSingleProperty(Type rootType, Type keyType,
            LambdaExpression pathToSecuredRelationship, WhenNull whenNull)
        {
            // We are building one of the below expression factories
            // ids => return x => x.SingleProperty == null || ids.Contains(x.SingleProperty.Id)
            // ids => return x => ids.Contains(x.SingleProperty.Id)

            // Create the constructed generic type of Func<T, bool> without having access to a generic parameter of T ( the parameter rootType is the result of typeof(T) )
            var typeForRootPredicate = BuildFuncOfTBool(rootType);

            // Create an Expression to turn the parameter into an accessor like {parameter of rootType}.SingleProperty.Id, when combined in a lambda this will become {parameter of rootType} => {parameter of rootType}.SingleProperty.Id
            MemberExpression accessorForSingleId;
            if (pathToSecuredRelationship.ReturnType.IsValueType)
            {
                accessorForSingleId = (MemberExpression) pathToSecuredRelationship.Body;
            }
            else
            {
                accessorForSingleId = Expression.PropertyOrField(pathToSecuredRelationship.Body, "Id");
            }

            // Create a parameter that represents the type of list of secured Ids that your user is entitled to access, this is IEnumerable<keyType>
            var typeForEntitledIdsParameter = BuildEnumerableOfT(keyType);

            // Create a parameter that represents the of list of secured Ids that your user is entitled to access, created from the above type
            var parameterForEntitledIds = BuildIdsParameter(typeForEntitledIdsParameter);

            // Reflect the generic method infromation from the keyType for checking later. This will be IEnumberable<keyType>.Contains({parameter of keyType}) 
            var containsMethod = BuildContainsMethod(keyType, 2);

            // Create an expression that given a collection of Ids calls contains, and passes the Id accessor from above. Outputs - ids.Contains({parameter of rootType}.Id)
            var containsMethodCall = Expression.Call(null, containsMethod, parameterForEntitledIds, accessorForSingleId);

            // Create a placeholder for variations of which where predicate we are going to need based on null behavior.
            Expression wherePredicate;
            switch (whenNull)
            {
                // When we find a null object, the security expression should treat this the same as finding an object the user has permissions to
                case WhenNull.Allow:
                {
                    // Create a check for property = null and add it in an or to the lambda
                    var nullCheckOnProp = Expression.Equal(pathToSecuredRelationship.Body,
                        Expression.Constant(null, pathToSecuredRelationship.ReturnType));
                    // Create the lambda ( x => x.SingleProperty == null || ids.Contains(x.SingleProperty.Id) )
                    wherePredicate = Expression.Lambda(typeForRootPredicate,
                        Expression.OrElse(nullCheckOnProp, containsMethodCall), pathToSecuredRelationship.Parameters);
                }
                    break;
                // When we find a null object, the security expression should treat this the same as finding an object that the user doesn't have permission to
                case WhenNull.Deny:
                {
                    // Create the lambda ( x => ids.Contains(x.SingleProperty.Id) )
                    wherePredicate = Expression.Lambda(typeForRootPredicate, containsMethodCall,
                        pathToSecuredRelationship.Parameters);
                }
                    break;
                default:
                {
                    throw new NotImplementedException(
                        "Allow and Deny are the only supported behaviors, stop modifying the enum without fixing the expression generator too!!");
                }
            }
            // Return a expression that when compiled will build a where predicate on execution. ids => { predicate that takes ids from above }
            return Expression.Lambda(Expression.Quote(wherePredicate), parameterForEntitledIds);
        }

        public static LambdaExpression BuildPredicateFactoryForSingleProperty<T, TK, TId>(
            Expression<Func<T, TK>> pathToSecuredRelationship, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            return BuildPredicateFactoryForSingleProperty(typeof (T), typeof (TId), pathToSecuredRelationship, whenNull);
        }

        private static Expression BuildAllowWhenNullWherePredicate(LambdaExpression pathToSecuredRelationship,
            Type typeForRootPredicate, MethodInfo anyPredicate, MethodInfo anyRecordCheck,
            LambdaExpression containsPredicate)
        {
            Expression wherePredicate;

            // Build !x.CollectionProperty.Any()
            var emptyCollectionCheck =
                Expression.Not(Expression.Call(null, anyRecordCheck, pathToSecuredRelationship.Body));

            // Build x.CollectionProperty.Any(c => ids.Contains(c.Id))
            var anyCall = Expression.Call(null, anyPredicate, pathToSecuredRelationship.Body, containsPredicate);

            // Combine two predicates into x => !x.CollectionProperty.Any() || x.CollectionProperty.Any(c => ids.Contains(c.Id))
            wherePredicate = Expression.Lambda(typeForRootPredicate, Expression.OrElse(emptyCollectionCheck, anyCall),
                pathToSecuredRelationship.Parameters);
            return wherePredicate;
        }

        private static Expression BuildDenyWhenNullWherePredicate(LambdaExpression pathToSecuredRelationship,
            Type typeForRootPredicate, MethodInfo anyRecordCheck, MethodInfo anyPredicate,
            LambdaExpression containsPredicate)
        {
            Expression wherePredicate;

            // Build x.CollectionProperty.Any()
            var notEmptyCollectionCheck = Expression.Call(null, anyRecordCheck, pathToSecuredRelationship.Body);

            // Build x.CollectionProperty.Any(c => ids.Contains(c.Id))
            var anyCall = Expression.Call(null, anyPredicate, pathToSecuredRelationship.Body, containsPredicate);

            // Combine two predicates into x => x.CollectionProperty.Any() && x.CollectionProperty.Any(c => ids.Contains(c.Id))
            wherePredicate = Expression.Lambda(typeForRootPredicate,
                Expression.AndAlso(notEmptyCollectionCheck, anyCall), pathToSecuredRelationship.Parameters);
            return wherePredicate;
        }

        private static MethodInfo GetEnumerableMethod(string methodName, int parameterCount)
        {
            return
                typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Single(mi => mi.Name == methodName && mi.GetParameters().Count() == parameterCount);
        }

        private static MethodInfo BuildAnyMethod(Type typeInCollection, int parameterCount)
        {
            return GetEnumerableMethod("Any", parameterCount).MakeGenericMethod(typeInCollection);
        }

        private static MethodInfo BuildContainsMethod(Type keyType, int parameterCount)
        {
            return GetEnumerableMethod("Contains", parameterCount).MakeGenericMethod(keyType);
        }

        private static Type BuildEnumerableOfT(Type keyType)
        {
            return typeof (IEnumerable<>).GetGenericTypeDefinition().MakeGenericType(keyType);
        }

        private static Type BuildFuncOfTBool(Type rootType)
        {
            return typeof (Func<,>).GetGenericTypeDefinition().MakeGenericType(rootType, typeof (bool));
        }

        private static ParameterExpression BuildIdsParameter(Type typeForEntitledIdsParameter)
        {
            return Expression.Parameter(typeForEntitledIdsParameter, "Ids");
        }
    }
}