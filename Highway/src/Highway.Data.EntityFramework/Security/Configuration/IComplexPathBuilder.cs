using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public interface IComplexPathBuilder<T>
    {
        IComplexPathBuilder<T> Or<TK>(Expression<Func<T, TK>> propertyExpression);
        IComplexPathBuilder<T> Or<TK>(Expression<Func<T, TK>> propertyExpression, WhenNull nullBehavior);
        IComplexPathBuilder<T> And<TK>(Expression<Func<T, TK>> propertyExpression);
        IComplexPathBuilder<T> And<TK>(Expression<Func<T, TK>> propertyExpression, WhenNull nullBehavior);
    }
}