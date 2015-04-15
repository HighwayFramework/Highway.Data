using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public abstract class Relationship
    {
        public List<Type> SecuredBy { get; set; }
        public abstract Type Secured { get; }

        public abstract IQueryable<T> ApplySecurity<T>(IQueryable<T> query, IProvideEntitlements entitlementProvider)
            where T : class;

        public abstract LambdaExpression GetPredicate(IProvideEntitlements entitlementProvider);
        public abstract bool ByPassSecurity<T>(IProvideEntitlements entitlementProvider);
    }
}