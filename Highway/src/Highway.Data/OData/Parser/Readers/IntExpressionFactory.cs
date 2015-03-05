using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class IntExpressionFactory : ValueExpressionFactoryBase<int>
    {
        public override ConstantExpression Convert(string token)
        {
            int number;
            if (int.TryParse(token, out number))
            {
                return Expression.Constant(number);
            }

            throw new FormatException("Could not read " + token + " as integer.");
        }
    }
}