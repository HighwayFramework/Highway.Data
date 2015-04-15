using System;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Expressions
{
    public static class ExpressionDetailBuilder
    {
        public static ExpressionDetail Build(Type securedType, Type secureByType, LambdaExpression propertyExpression,
            WhenNull whenNull)
        {
            var rootIdsExpression = CreateIdAccessorFunction(securedType, secureByType, propertyExpression);
            var predicateFactory =
                CreatePredicateFactory(securedType, secureByType, propertyExpression, whenNull).Compile();

            return new ExpressionDetail(predicateFactory, rootIdsExpression);
        }

        private static Delegate CreateIdAccessorFunction(Type securedType, Type securedByType,
            LambdaExpression propertyExpression)
        {
            if (securedByType == securedType)
            {
                var keyType = GetKeyType(securedType);
                return SecuredIdSelectBuilder.BuildSelectorForSelf(securedType, keyType).Compile();
            }
            if (securedByType.IsCollection() || securedByType.IsEnumerable())
            {
                var keyType = GetKeyType(securedByType.ToSingleType());
                return
                    SecuredIdSelectBuilder.BuildSelectorForCollection(securedType, securedByType.ToSingleType(), keyType,
                        propertyExpression).Compile();
            }
            else
            {
                var keyType = GetKeyType(securedByType);
                return
                    SecuredIdSelectBuilder.BuildSelectorForSingleProperty(securedType, securedByType, keyType,
                        propertyExpression).Compile();
            }
        }

        private static LambdaExpression CreatePredicateFactory(Type securedType, Type securedByType,
            LambdaExpression propertyExpression, WhenNull whenNull)
        {
            if (securedByType == securedType)
            {
                var keyType = GetKeyType(securedType);
                return SecuredPredicateBuilder.BuildPredicateFactoryForSelf(securedType, keyType);
            }
            if (securedByType.IsCollection() || securedByType.IsEnumerable())
            {
                var keyType = GetKeyType(securedByType.ToSingleType());
                return SecuredPredicateBuilder.BuildPredicateFactoryForCollectionProperty(securedType, keyType,
                    propertyExpression, whenNull);
            }
            else
            {
                var keyType = GetKeyType(securedByType);
                return SecuredPredicateBuilder.BuildPredicateFactoryForSingleProperty(securedType, keyType,
                    propertyExpression, whenNull);
            }
        }

        private static Type GetKeyType(Type securedType)
        {
            var identifiableInterface =
                securedType.GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IIdentifiable<>));
            if (identifiableInterface == null)
            {
                throw new InvalidOperationException("Cannot map against non-identifiable objects");
            }
            return identifiableInterface.GenericTypeArguments.Single();
        }
    }
}