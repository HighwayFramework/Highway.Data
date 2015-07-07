using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;

namespace Highway.Data.OData.Parser.Readers
{
    internal class DateTimeOffsetExpressionFactory : ValueExpressionFactoryBase<DateTimeOffset>
    {
        private static readonly Regex DateTimeOffsetRegex =
            new Regex(
                @"datetimeoffset['\""](\d{4}\-\d{2}\-\d{2}(T\d{2}\:\d{2}\:\d{2}(\.\d+)?)?([\-\+]\d{2}:\d{2}|Z)?)['\""]",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override ConstantExpression Convert(string token)
        {
            var match = DateTimeOffsetRegex.Match(token);
            if (match.Success)
            {
                var dateTimeOffset = XmlConvert.ToDateTimeOffset(match.Groups[1].Value);
                return Expression.Constant(dateTimeOffset);
            }

            throw new FormatException("Could not read " + token + " as DateTimeOffset.");
        }
    }
}