using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public abstract class PathDescriptor
    {
        public abstract IEnumerable<Type> Secured { get; }
        public abstract IEnumerable<Type> SecuredBy { get; }
        public abstract LambdaExpression BuildPredicate(IProvideEntitlements entitlementProvider);
        public abstract IEnumerable<Delegate> BuildRootAccessExpression();
        public abstract bool ByPassSecurity<T>(IProvideEntitlements entitlementProvider);
    }
}