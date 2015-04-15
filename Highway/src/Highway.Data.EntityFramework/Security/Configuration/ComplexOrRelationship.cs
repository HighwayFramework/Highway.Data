using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Extensions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class ComplexOrRelationship : Relationship
    {
        private readonly Relationship _left;
        private readonly Relationship _right;

        public ComplexOrRelationship(Relationship left, Relationship right)
        {
            _left = left;
            _right = right;
            SecuredBy = new List<Type>();
            SecuredBy.AddRange(_left.SecuredBy);
            SecuredBy.AddRange(_right.SecuredBy);
        }

        public override Type Secured
        {
            get { return _left.Secured; }
        }

        public override IQueryable<T> ApplySecurity<T>(IQueryable<T> query, IProvideEntitlements entitlementProvider)
        {
            if (ByPassSecurity<T>(entitlementProvider))
            {
                return query;
            }
            var combinedFilter =
                (Expression<Func<T, bool>>)
                    CombineToLambda(_left.GetPredicate(entitlementProvider), _right.GetPredicate(entitlementProvider));
            return query.Where(combinedFilter);
        }

        public override LambdaExpression GetPredicate(IProvideEntitlements entitlementProvider)
        {
            return CombineToLambda(_left.GetPredicate(entitlementProvider), _right.GetPredicate(entitlementProvider));
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