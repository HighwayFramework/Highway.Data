using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class SimplePathDescriptor : PathDescriptor
    {
        private readonly ExpressionDetail _details;
        private readonly Type _securedByType;
        private readonly Type _securedType;

        public SimplePathDescriptor(Type securedType, Type securedByType, LambdaExpression propertyExpression,
            WhenNull whenNull)
        {
            _securedType = securedType;
            _securedByType = securedByType;
            _details = ExpressionDetailBuilder.Build(securedType, securedByType, propertyExpression, whenNull);
        }

        public override IEnumerable<Type> Secured
        {
            get { return new[] {_securedType}; }
        }

        public override IEnumerable<Type> SecuredBy
        {
            get { return new[] {_securedByType}; }
        }

        public override bool ByPassSecurity<T>(IProvideEntitlements entitlementProvider)
        {
            return entitlementProvider.IsEntitledToAll(_securedByType);
        }

        public override LambdaExpression BuildPredicate(IProvideEntitlements entitlementProvider)
        {
            var entitledIds = entitlementProvider.GetEntitledIds(Extensions.TypeExtensions.ToSingleType(_securedByType));
            if (entitledIds == null || !entitledIds.Any())
            {
                throw new InvalidOperationException("Cannot apply security for type when you has no entitlement");
            }
            var lambdaExpression = _details.PredicateFactory.DynamicInvoke(entitledIds);
            return (LambdaExpression) lambdaExpression;
        }

        public override IEnumerable<Delegate> BuildRootAccessExpression()
        {
            return new[] {_details.RootIdsExpression};
        }
    }
}