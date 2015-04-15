using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public interface IRootBuilder<T>
    {
        IParentRootBuilder<T> WithChildren(
            Expression<Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>>> pathDescriptor);

        IChildRootBuilder<T> WithParent(Expression<Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>>> pathDescriptor);
    }
}