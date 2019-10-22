using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class BooleanExpressionFactory : ValueExpressionFactoryBase<bool>
    {
        private static readonly Regex TrueRegex = new Regex("1|true", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex FalseRegex = new Regex("0|false", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public override ConstantExpression Convert(string token)
        {
            if (TrueRegex.IsMatch(token))
            {
                return Expression.Constant(true);
            }

            if (FalseRegex.IsMatch(token))
            {
                return Expression.Constant(false);
            }

            return Expression.Constant(null);
        }
    }
}