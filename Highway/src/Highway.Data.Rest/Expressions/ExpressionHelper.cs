using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.Rest.Expressions
{
    public static class ExpressionHelper
    {
        public static Expression<Func<object, object>> ConvertParameterToObject<T,TK>(this Expression<Func<T, TK>> source)
        {
            return source.ReplaceParameterWithBase<T, TK, object>();
        }

        public static Expression<Func<TBase, TBase>> ReplaceParameterWithBase<T, TResult, TBase>(this Expression<Func<T, TResult>> lambda)
            where T : TBase
        {
            var param = lambda.Parameters.Single();
            return (Expression<Func<TBase, TBase>>)
                ParameterRebinder.ReplaceParameters(new Dictionary<ParameterExpression, ParameterExpression>
                                                {
                                                    { param, Expression.Parameter(typeof (TBase), param.Name) }
                                                }, lambda.Body);
        }
    }
}