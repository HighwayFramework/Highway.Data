using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class DoubleExpressionFactory : ValueExpressionFactoryBase<double>
    {
        public override ConstantExpression Convert(string token)
        {
            double number;
            if (double.TryParse(token.Trim('D', 'd'), NumberStyles.Any, CultureInfo.InvariantCulture, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as double.");
        }
    }
}