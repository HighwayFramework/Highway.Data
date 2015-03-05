using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class DecimalExpressionFactory : ValueExpressionFactoryBase<decimal>
    {
        public override ConstantExpression Convert(string token)
        {
            decimal number;
            if (decimal.TryParse(token.Trim('M', 'm'), NumberStyles.Any, CultureInfo.InvariantCulture, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as decimal.");
        }
    }
}