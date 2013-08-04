using System;
using System.Linq.Expressions;

namespace Highway.Data.Rest.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, TK>> ToExpression<T,TK>(this Func<T, TK> f)
        {
            return x => f(x);
        }
    }
}