using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class LongExpressionFactory : ValueExpressionFactoryBase<long>
    {
        public override ConstantExpression Convert(string token)
        {
            long number;
            if (long.TryParse(token, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as Long.");
        }
    }
}