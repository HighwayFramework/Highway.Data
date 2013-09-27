using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Linq.Expressions;

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
