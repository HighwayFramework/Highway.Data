using System;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class SimpleRelationship : Relationship
    {
        private readonly PathDescriptor _pathDescriptor;
        private readonly Type _secured;

        public SimpleRelationship(PathDescriptor pathDescriptor, Type secured)
        {
            _pathDescriptor = pathDescriptor;
            _secured = secured;
            SecuredBy = pathDescriptor.SecuredBy.ToList();
        }

        public override Type Secured
        {
            get { return _secured; }
        }

        public override IQueryable<T> ApplySecurity<T>(IQueryable<T> query, IProvideEntitlements entitlementProvider)
        {
            if (ByPassSecurity<T>(entitlementProvider))
            {
                return query;
            }
            var combinedFilter = (Expression<Func<T, bool>>) _pathDescriptor.BuildPredicate(entitlementProvider);
            return query.Where(combinedFilter);
        }

        public override LambdaExpression GetPredicate(IProvideEntitlements entitlementProvider)
        {
            return _pathDescriptor.BuildPredicate(entitlementProvider);
        }

        public override bool ByPassSecurity<T>(IProvideEntitlements entitlementProvider)
        {
            return _pathDescriptor.ByPassSecurity<T>(entitlementProvider);
        }
    }
}