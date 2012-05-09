using System;
using System.Linq.Expressions;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.QueryObjects
{
    public static class QueryExtensions
    {
        public static IQuery<T> Take<T>(this IQuery<T> extend, int count)
        {
            var generics = new Type[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Take", generics, parameters);
            return extend;
        }

        public static IQuery<T> Skip<T>(this IQuery<T> extend, int count)
        {
            var generics = new Type[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Skip", generics, parameters);
            return extend;
        }
    }
}