using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Highway.Data.OData.Extensions
{
    public static class ExpressionExtensions
    {
        private static readonly Regex NumberRegex = new Regex("^(\\d+)$", RegexOptions.Compiled);

        public static Expression UnwrapEnumValue(this Expression right, Type leftType)
        {
            var constantExpression = right as ConstantExpression;
            if (constantExpression != null)
            {
                var rightValue = constantExpression.Value;
                if (constantExpression.Type == leftType)
                {
                    return constantExpression;
                }
                if (NumberRegex.IsMatch(rightValue.ToString()))
                {
                    var underlyingType = Enum.GetUnderlyingType(leftType);;
                    var numericValue = Convert.ChangeType(rightValue.ToString(), underlyingType);
                    var enumValue = Enum.ToObject(leftType, numericValue);
                    return Expression.Constant(enumValue);
                }
                var typedValue = Enum.Parse(leftType, rightValue.ToString());
                return Expression.Constant(typedValue);
            }
            throw new InvalidOperationException("You can only unwrap a constant expression");
        }
    }
}