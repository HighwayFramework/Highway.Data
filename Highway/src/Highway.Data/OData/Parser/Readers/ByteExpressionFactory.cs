
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class ByteExpressionFactory : ValueExpressionFactoryBase<byte>
    {
        public override ConstantExpression Convert(string token)
        {
            byte number;
            if (byte.TryParse(token, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as byte.");
        }
    }
}