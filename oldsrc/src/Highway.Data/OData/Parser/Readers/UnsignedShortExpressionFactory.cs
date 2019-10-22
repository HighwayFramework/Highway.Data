using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class UnsignedShortExpressionFactory : ValueExpressionFactoryBase<ushort>
    {
        public override ConstantExpression Convert(string token)
        {
            ushort number;
            if (ushort.TryParse(token, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as Unsigned Short.");
        }
    }
}