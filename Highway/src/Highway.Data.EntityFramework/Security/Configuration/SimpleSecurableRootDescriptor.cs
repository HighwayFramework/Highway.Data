using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class SimpleSecurableRootDescriptor<T> : RootDescriptor
    {
        public SimpleSecurableRootDescriptor()
        {
            SecuredBy = typeof (T);
            Expression<Func<T, T>> selfExpression = x => x;
            SelfExpression = selfExpression;
        }
    }
}