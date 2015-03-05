using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class GuidExpressionFactory : ValueExpressionFactoryBase<Guid>
    {
        private static readonly Regex GuidRegex = new Regex(@"guid['\""]([a-f0-9\-]+)['\""]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override ConstantExpression Convert(string token)
        {
            var match = GuidRegex.Match(token);
            if (match.Success)
            {
                Guid guid;
                if (Guid.TryParse(match.Groups[1].Value, out guid))
                {
                    return Expression.Constant(guid);
                }
            }

            throw new FormatException("Could not read " + token + " as Guid.");
        }
    }
}