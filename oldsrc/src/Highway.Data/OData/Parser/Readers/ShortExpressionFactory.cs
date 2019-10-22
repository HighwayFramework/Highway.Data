using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class ShortExpressionFactory : ValueExpressionFactoryBase<short>
    {
        public override ConstantExpression Convert(string token)
        {
            short number;
            if (short.TryParse(token, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as Short.");
        }
    }
}