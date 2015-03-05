using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class UnsignedIntExpressionFactory : ValueExpressionFactoryBase<uint>
    {
        public override ConstantExpression Convert(string token)
        {
            uint number;
            if (uint.TryParse(token, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as Unsigned Integer.");
        }
    }
}