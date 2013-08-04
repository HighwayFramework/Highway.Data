using System;
using System.Linq.Expressions;

namespace Highway.Data.Rest.Expressions
{
    public class TypeOf<T>
    {
        public static string Property<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;
            return body.Member.Name;
        }
    }
}