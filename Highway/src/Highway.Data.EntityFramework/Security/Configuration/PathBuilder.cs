using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class PathBuilder<T> : ISimplePathBuilder<T>, IComplexPathBuilder<T>
    {
        private PathDescriptor _pathDescriptor;

        public IComplexPathBuilder<T> Or<TK>(Expression<Func<T, TK>> propertyExpression)
        {
            return Or(propertyExpression, WhenNull.Deny);
        }

        public IComplexPathBuilder<T> Or<TK>(Expression<Func<T, TK>> propertyExpression, WhenNull nullBehavior)
        {
            _pathDescriptor = new ComplexOrPathDescriptor(_pathDescriptor,
                new SimplePathDescriptor(typeof (T), typeof (TK), propertyExpression, nullBehavior));
            return this;
        }

        public IComplexPathBuilder<T> And<TK>(Expression<Func<T, TK>> propertyExpression)
        {
            return And(propertyExpression, WhenNull.Deny);
        }

        public IComplexPathBuilder<T> And<TK>(Expression<Func<T, TK>> propertyExpression, WhenNull nullBehavior)
        {
            _pathDescriptor = new ComplexAndPathDescriptor(_pathDescriptor,
                new SimplePathDescriptor(typeof (T), typeof (TK), propertyExpression, nullBehavior));
            return this;
        }

        public IComplexPathBuilder<T> Path<TK>(Expression<Func<T, TK>> propertyExpression)
        {
            return Path(propertyExpression, WhenNull.Deny);
        }

        public IComplexPathBuilder<T> Path<TK>(Expression<Func<T, TK>> propertyExpression, WhenNull nullBehavior)
        {
            _pathDescriptor = new SimplePathDescriptor(typeof (T), typeof (TK), propertyExpression, nullBehavior);
            return this;
        }

        public PathDescriptor Build()
        {
            return _pathDescriptor;
        }
    }
}