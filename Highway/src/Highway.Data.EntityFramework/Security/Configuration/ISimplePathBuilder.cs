using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public interface ISimplePathBuilder<T>
    {
        IComplexPathBuilder<T> Path<TK>(Expression<Func<T, TK>> propertyExpression);
        IComplexPathBuilder<T> Path<TK>(Expression<Func<T, TK>> propertyExpression, WhenNull nullBehavior);
    }
}