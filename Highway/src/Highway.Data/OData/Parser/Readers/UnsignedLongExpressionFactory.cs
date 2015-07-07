using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class UnsignedLongExpressionFactory : ValueExpressionFactoryBase<ulong>
    {
        public override ConstantExpression Convert(string token)
        {
            ulong number;
            if (ulong.TryParse(token, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as Unsigned Long.");
        }
    }
}