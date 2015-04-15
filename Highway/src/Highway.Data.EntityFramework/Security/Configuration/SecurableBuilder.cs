using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class SecurableBuilder<T> : IRootBuilder<T>, IParentRootBuilder<T>, IChildRootBuilder<T>,
        IRelationshipBuilder
    {
        private RootDescriptor _rootDescriptor;

        public SecurableBuilder()
        {
            _rootDescriptor = new SimpleSecurableRootDescriptor<T>();
        }

        public Relationship Build()
        {
            return _rootDescriptor.Build();
        }

        public IParentRootBuilder<T> WithChildren(
            Expression<Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>>> pathDescriptor)
        {
            _rootDescriptor = new ParentSecurableRootDescriptor<T>(pathDescriptor);
            return this;
        }

        public IChildRootBuilder<T> WithParent(
            Expression<Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>>> pathDescriptor)
        {
            _rootDescriptor = new ChildSecurableRootDescriptor<T>(pathDescriptor);
            return this;
        }
    }
}