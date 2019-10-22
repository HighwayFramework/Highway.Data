using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;

namespace Highway.Data.OData.Parser.Readers
{
    internal class TimeSpanExpressionFactory : ValueExpressionFactoryBase<TimeSpan>
    {
        private static readonly Regex TimeSpanRegex = new Regex(@"^time['\""](P.+)['\""]$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override ConstantExpression Convert(string token)
        {
            var match = TimeSpanRegex.Match(token);
            if (match.Success)
            {
                try
                {
                    var timespan = XmlConvert.ToTimeSpan(match.Groups[1].Value);
                    return Expression.Constant(timespan);
                }
                catch
                {
                    throw new FormatException("Could not read " + token + " as TimeSpan.");
                }
            }

            throw new FormatException("Could not read " + token + " as TimeSpan.");
        }
    }
}