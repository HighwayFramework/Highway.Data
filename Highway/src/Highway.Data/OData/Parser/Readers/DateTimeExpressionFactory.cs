using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;

namespace Highway.Data.OData.Parser.Readers
{
    internal class DateTimeExpressionFactory : ValueExpressionFactoryBase<DateTime>
    {
        private static readonly Regex DateTimeRegex =
            new Regex(@"datetime['\""](\d{4}\-\d{2}\-\d{2}(T\d{2}\:\d{2}\:\d{2}(.\d+)?)?(?<z>Z)?)['\""]",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override ConstantExpression Convert(string token)
        {
            var match = DateTimeRegex.Match(token);
            if (match.Success)
            {
                var dateTime = match.Groups["z"].Success
                    ? XmlConvert.ToDateTime(match.Groups[1].Value, XmlDateTimeSerializationMode.Utc)
                    : DateTime.SpecifyKind(DateTime.Parse(match.Groups[1].Value), DateTimeKind.Unspecified);
                return Expression.Constant(dateTime);
            }

            throw new FormatException("Could not read " + token + " as DateTime.");
        }
    }
}