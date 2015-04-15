using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Extensions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class ComplexOrPathDescriptor : PathDescriptor
    {
        private readonly PathDescriptor _left;
        private readonly PathDescriptor _right;
        private readonly List<Type> _secured = new List<Type>();
        private readonly List<Type> _securedBy = new List<Type>();

        public ComplexOrPathDescriptor(PathDescriptor left, PathDescriptor right)
        {
            _left = left;
            _right = right;
            _secured.AddRange(_left.Secured);
            _secured.AddRange(_right.Secured);
            _securedBy.AddRange(_left.SecuredBy);
            _securedBy.AddRange(_right.SecuredBy);
        }

        public override IEnumerable<Type> Secured
        {
            get { return _secured; }
        }

        public override IEnumerable<Type> SecuredBy
        {
            get { return _securedBy; }
        }

        public override LambdaExpression BuildPredicate(IProvideEntitlements entitlementProvider)
        {
            return CombineToLambda(_left.BuildPredicate(entitlementProvider), _right.BuildPredicate(entitlementProvider));
        }

        public override IEnumerable<Delegate> BuildRootAccessExpression()
        {
            var delegates = new List<Delegate>();
            delegates.AddRange(_left.BuildRootAccessExpression());
            delegates.AddRange(_right.BuildRootAccessExpression());
            return delegates;
        }

        public override bool ByPassSecurity<T>(IProvideEntitlements entitlementProvider)
        {
            return _left.ByPassSecurity<T>(entitlementProvider) || _right.ByPassSecurity<T>(entitlementProvider);
        }

        private LambdaExpression CombineToLambda(LambdaExpression leftExpression, LambdaExpression rightExpression)
        {
            return leftExpression.OrElse(rightExpression);
        }
    }
}