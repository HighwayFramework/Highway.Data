#region

using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Highway.Data.Rest.Expressions
{
    public class TypeOf<T>
    {
        public static string Property<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body != null) return body.Member.Name;
            throw new InvalidExpressionException("Expression was not a member expression");
        }

        public static PropertyInfo PropertyInfo<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null) throw new InvalidExpressionException("Expression was not a member expression");
            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new InvalidExpressionException(string.Format("Key for Type {0} must be a property expressions",
                    typeof (T).Name));
            return propertyInfo;
        }

        public static PropertyInfo PropertyInfo(string propertyName)
        {
            var type = typeof (T);
            var property = type.GetProperty(propertyName);
            if (property == null)
                throw new InvalidOperationException(string.Format("Key Property {0} for type {1} doesn't exist",
                    type.Name, propertyName));
            return property;
        }
    }
}