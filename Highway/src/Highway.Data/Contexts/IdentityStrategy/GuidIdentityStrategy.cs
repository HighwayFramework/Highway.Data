#region

using System;
using System.Linq.Expressions;

#endregion

namespace Highway.Data.Contexts
{
    public class GuidIdentityStrategy<T> : IdentityStrategy<T, Guid>
        where T : class
    {
        static GuidIdentityStrategy()
        {
            Generator = Guid.NewGuid;
        }

        public GuidIdentityStrategy(Expression<Func<T, Guid>> property)
            : base(property)
        {
        }
    }
}