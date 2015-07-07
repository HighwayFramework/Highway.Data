using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class SingleExpressionFactory : ValueExpressionFactoryBase<float>
    {
        public override ConstantExpression Convert(string token)
        {
            float number;
            if (float.TryParse(token.Trim('F', 'f'), NumberStyles.Any, CultureInfo.InvariantCulture, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as short.");
        }
    }
}