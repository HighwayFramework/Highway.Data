using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    /// <summary>
    /// This class uses expressions and reflection to build predicate factories based on security and  mappings.
    /// If you want to modify this class you need to be proficient in:
    /// 
    /// Reading and understanding LINQ
    /// Reading, Modifying, and creating Expressions
    /// Understanding closures and delegates
    /// Understanding the Factory pattern and deferred execution
    /// 
    /// If you do not hold these skills, do not modify this code.
    /// </summary>
    public static class PredicateBuilder
    {
        public static LambdaExpression BuildPredicateFactoryForCollectionProperty(Type rootType, Type typeInCollection, Type keyType, LambdaExpression pathToRelationship, WhenNull whenNull)
        {
            // We are building one of the below expression factories
            // ids => return x => !x.CollectionProperty.Any() || x.CollectionProperty.Any(c => ids.Contains(c.Id))
            // ids => return x => x.CollectionProperty.Any() && x.CollectionProperty.Any(c => ids.Contains(c.Id))
            // Create the constructed generic type of Func<T, bool> without having access to a generic parameter of T ( the parameter rootType is the result of typeof(T) )
            var typeForRootPredicate = MakeFuncOfTBool(rootType);

            // Create a parameter that represents the object from the collection, this will later become the {parameter of typeInCollection} => part of a lambda
            var parameterForSingleId = Expression.Parameter(typeInCollection);
            
            // Create an Expression to turn the parameter into an accessor like {parameter of typeInCollection}.Id, when combined in a lambda this will become {parameter of typeInCollection} => {parameter of typeInCollection}.Id
            var accessorForSingleId = Expression.PropertyOrField(parameterForSingleId, "Id");

            // Reflect the generic method information from the keyType for checking later. This will be IEnumberable<keyType>.Contains({parameter of keyType}) 
            var containsMethod = MakeContainsMethod(keyType, 2);
            
            // Create a parameter that represents the type of list of  Ids that your user is entitled to access, this is IEnumerable<keyType>
            var typeForEntitledIdsParameter = MakeEnumerableOfT(keyType);
            
            // Create a parameter that represents the list of  Ids that your user is entitled to access, created from the above type
            var parameterForEntitledIds = MakeIdsParameter(typeForEntitledIdsParameter);

            // Reflect LINQ methods to get the .Any() and .Any(x => [lambda predicate here]) methods
            var reflectedMethodInfoForLinqAnyThatDoesNotTakeParameters = MakeAnyMethod(typeInCollection, 1);
            var reflectedMethodInfoForLinqAnyThatTakesAPredicate = MakeAnyMethod(typeInCollection, 2);

            // Create an expression that given a collection of Ids calls contains, and passes the Id accessor from above. Outputs - ids.Contains({parameter of typeInCollection}.Id)
            var containsMethodCall = Expression.Call(null, containsMethod, parameterForEntitledIds, accessorForSingleId);

            // Create a lambda from the above method call that closures in a reference to the type in collection. Outputs - {parameter of typeInCollection} => ids.Contains({parameter of typeInCollection}.Id)
            var containsPredicate = Expression.Lambda(containsMethodCall, parameterForSingleId);

            // Create a placeholder for variations of which where predicate we are going to need based on null behavior.
            Expression wherePredicate;
            switch (whenNull)
            {
                // When we find a null object, the security expression should treat this the same as finding an object the user has permissions to
                case WhenNull.Allow:
                    {
                        // Build the follow predicate x => !x.CollectionProperty.Any() || x.CollectionProperty.Any(c => ids.Contains(c.Id))
                        wherePredicate = BuildAllowWhenNullWherePredicate(pathToRelationship, typeForRootPredicate, reflectedMethodInfoForLinqAnyThatTakesAPredicate, reflectedMethodInfoForLinqAnyThatDoesNotTakeParameters, containsPredicate);
                    }
                    break;
                // When we find a null object, the security expression should treat this the same as finding an object that the user doesn't have permission to
                case WhenNull.Deny:
                    {
                        // Build the following predicate x => x.CollectionProperty.Any() && x.CollectionProperty.Any(c => ids.Contains(c.Id))
                        wherePredicate = BuildDenyWhenNullWherePredicate(pathToRelationship, typeForRootPredicate, reflectedMethodInfoForLinqAnyThatDoesNotTakeParameters, reflectedMethodInfoForLinqAnyThatTakesAPredicate, containsPredicate);
                    }
                    break;
                default:
                    {
                        throw new NotImplementedException("Allow and Deny are the only supported behaviors.  Stop modifying the enum without fixing the expression generator too!!");
                    }
            }

            // Return a expression that when compiled will build a where predicate on execution. ids => { predicate that takes ids from above }
            return Expression.Lambda(Expression.Quote(wherePredicate), parameterForEntitledIds);
        }

        public static LambdaExpression BuildPredicateFactoryForCollectionProperty<T, TK, TId>(Expression<Func<T, IEnumerable<TK>>> pathToRelationship, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            return BuildPredicateFactoryForCollectionProperty(typeof(T), typeof(TK), typeof(TId), pathToRelationship, whenNull);
        }

        public static LambdaExpression BuildPredicateFactoryForSelf(Type rootType, Type keyType)
        {
            // We are building one of the below expressions
            // ids => return x => ids.Contains(x.Id)
            // Create a parameter that represents the object, this will later become the {parameter of rootType} => part of a lambda
            var parameterForSingleId = Expression.Parameter(rootType);

            // Create an Expression to turn the parameter into an accessor like {parameter of rootType}.Id, when combined in a lambda this will become {parameter of rootType} => {parameter of rootType}.Id
            var accessorForSingleId = Expression.PropertyOrField(parameterForSingleId, "Id");

            // Create a parameter that represents the type of list of  Ids that your user is entitled to access, this is IEnumerable<keyType>
            var typeForEntitledIdsParameter = MakeEnumerableOfT(keyType);

            // Create a parameter that represents the of list of  Ids that your user is entitled to access, created from the above type
            var parameterForEntitledIds = MakeIdsParameter(typeForEntitledIdsParameter);

            // Reflect the generic method infromation from the keyType for checking later. This will be IEnumberable<keyType>.Contains({parameter of keyType}) 
            var containsMethod = MakeContainsMethod(keyType, 2);

            // Create an expression that given a collection of Ids calls contains, and passes the Id accessor from above. Outputs - ids.Contains({parameter of rootType}.Id)
            var containsMethodCall = Expression.Call(null, containsMethod, parameterForEntitledIds, accessorForSingleId);

            // Build a Func<IEnumerable<keyType>, bool> that will be used to wrap the Contains expression, so we can closure the reference to the Ids collection provided.
            // This will produce the x => ids.Contains(x.Id) lambda type.
            var containsLambdaType = MakeFuncOfTBool(typeForEntitledIdsParameter);

            // build the actual lambda, providing the Contains method call expressions
            var containsMethodLambda = Expression.Lambda(containsLambdaType, containsMethodCall, parameterForEntitledIds);

            // Create a lambda from the above method call that closures in a reference to the type.
            return Expression.Lambda(Expression.Quote(containsMethodLambda), parameterForSingleId);
        }

        public static LambdaExpression BuildPredicateFactoryForSelf<T, TId>()
        {
            return BuildPredicateFactoryForSelf(typeof(T), typeof(TId));
        }

        public static LambdaExpression BuildPredicateFactoryForSingleProperty(Type rootType, Type typeInProperty, Type keyType, LambdaExpression pathToRelationship, WhenNull whenNull)
        {
            // We are building one of the below expression factories
            // ids => return x => x.SingleProperty == null || ids.Contains(x.SingleProperty.Id)
            // ids => return x => ids.Contains(x.SingleProperty.Id)

            // Create the constructed generic type of Func<T, bool> without having access to a generic parameter of T ( the parameter rootType is the result of typeof(T) )
            var typeForRootPredicate = MakeFuncOfTBool(rootType);

            // Create an Expression to turn the parameter into an accessor like {parameter of rootType}.SingleProperty.Id, when combined in a lambda this will become {parameter of rootType} => {parameter of rootType}.SingleProperty.Id
            var accessorForSingleId = Expression.PropertyOrField(pathToRelationship.Body, "Id");

            // Create a parameter that represents the type of list of  Ids that your user is entitled to access, this is IEnumerable<keyType>
            var typeForEntitledIdsParameter = MakeEnumerableOfT(keyType);

            // Create a parameter that represents the of list of  Ids that your user is entitled to access, created from the above type
            var parameterForEntitledIds = MakeIdsParameter(typeForEntitledIdsParameter);

            // Reflect the generic method infromation from the keyType for checking later. This will be IEnumberable<keyType>.Contains({parameter of keyType}) 
            var containsMethod = MakeContainsMethod(keyType, 2);

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
                        var nullCheckOnProp = Expression.Equal(pathToRelationship.Body, Expression.Constant(null, typeInProperty));
                        // Create the lambda ( x => x.SingleProperty == null || ids.Contains(x.SingleProperty.Id) )
                        wherePredicate = Expression.Lambda(typeForRootPredicate, Expression.OrElse(nullCheckOnProp, containsMethodCall), pathToRelationship.Parameters);
                    }
                    break;
                // When we find a null object, the security expression should treat this the same as finding an object that the user doesn't have permission to
                case WhenNull.Deny:
                    {
                        // Create the lambda ( x => ids.Contains(x.SingleProperty.Id) )
                        wherePredicate = Expression.Lambda(typeForRootPredicate, containsMethodCall, pathToRelationship.Parameters);
                    }
                    break;
                default:
                    {
                        throw new NotImplementedException("Allow and Deny are the only supported behaviors, stop modifying the enum without fixing the expression generator too!!");
                    }
            }
            // Return a expression that when compiled will build a where predicate on execution. ids => { predicate that takes ids from above }
            return Expression.Lambda(Expression.Quote(wherePredicate), parameterForEntitledIds);
        }

        public static LambdaExpression BuildPredicateFactoryForSingleProperty<T, TK, TId>(Expression<Func<T, TK>> pathToRelationship, WhenNull whenNull)
            where TK : IIdentifiable<TId>
            where TId : IEquatable<TId>
        {
            return BuildPredicateFactoryForSingleProperty(typeof(T), typeof(TK), typeof(TId), pathToRelationship, whenNull);
        }

        private static Expression BuildAllowWhenNullWherePredicate(LambdaExpression pathToRelationship, Type typeForRootPredicate, MethodInfo anyPredicate, MethodInfo anyRecordCheck, LambdaExpression containsPredicate)
        {
            Expression wherePredicate;
            // Build !x.CollectionProperty.Any()
            var emptyCollectionCheck = Expression.Not(Expression.Call(null, anyRecordCheck, pathToRelationship.Body));
            // Build x.CollectionProperty.Any(c => ids.Contains(c.Id))
            var anyCall = Expression.Call(null, anyPredicate, pathToRelationship.Body, containsPredicate);
            // Combine two predicates into x => !x.CollectionProperty.Any() || x.CollectionProperty.Any(c => ids.Contains(c.Id))
            wherePredicate = Expression.Lambda(typeForRootPredicate, Expression.OrElse(emptyCollectionCheck, anyCall), pathToRelationship.Parameters);
            return wherePredicate;
        }

        private static Expression BuildDenyWhenNullWherePredicate(LambdaExpression pathToRelationship, Type typeForRootPredicate, MethodInfo anyRecordCheck, MethodInfo anyPredicate, LambdaExpression containsPredicate)
        {
            Expression wherePredicate;
            // Build x.CollectionProperty.Any()
            var notEmptyCollectionCheck = Expression.Call(null, anyRecordCheck, pathToRelationship.Body);
            // Build x.CollectionProperty.Any(c => ids.Contains(c.Id))
            var anyCall = Expression.Call(null, anyPredicate, pathToRelationship.Body, containsPredicate);
            // Combine two predicates into x => x.CollectionProperty.Any() && x.CollectionProperty.Any(c => ids.Contains(c.Id))
            wherePredicate = Expression.Lambda(typeForRootPredicate, Expression.AndAlso(notEmptyCollectionCheck, anyCall), pathToRelationship.Parameters);
            return wherePredicate;
        }

        private static MethodInfo GetEnumerableMethod(string methodName, int parameterCount)
        {
            return typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(mi => mi.Name == methodName && mi.GetParameters().Count() == parameterCount);
        }

        private static MethodInfo MakeAnyMethod(Type typeInCollection, int parameterCount)
        {
            return GetEnumerableMethod("Any", parameterCount).MakeGenericMethod(typeInCollection);
        }

        private static MethodInfo MakeContainsMethod(Type keyType, int parameterCount)
        {
            return GetEnumerableMethod("Contains", parameterCount).MakeGenericMethod(keyType);
        }

        private static Type MakeEnumerableOfT(Type keyType)
        {
            return typeof(IEnumerable<>).GetGenericTypeDefinition().MakeGenericType(keyType);
        }

        private static Type MakeFuncOfTBool(Type rootType)
        {
            return typeof(Func<,>).GetGenericTypeDefinition().MakeGenericType(rootType, typeof(bool));
        }

        private static ParameterExpression MakeIdsParameter(Type typeForEntitledIdsParameter)
        {
            return Expression.Parameter(typeForEntitledIdsParameter, "Ids");
        }
    }
}
