using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class EnumExpressionFactory : IValueExpressionFactory
    {
        private static readonly Regex EnumRegex = new Regex("^(.+)'(.+)'$", RegexOptions.Compiled);
        private static readonly ConcurrentDictionary<string, Type> KnownTypes = new ConcurrentDictionary<string, Type>();

        public bool Handles(Type type)
        {
            return type.IsEnum;
        }

        public ConstantExpression Convert(string token)
        {
            var match = EnumRegex.Match(token);
            if (match.Success)
            {
                var type = KnownTypes.GetOrAdd(match.Groups[1].Value, LoadType);
                var value = match.Groups[2].Value;

                Contract.Assume(type != null);

                return Expression.Constant(Enum.Parse(type, value));
            }

            throw new FormatException("Could not read " + token + " as Enum.");
        }

        private Type LoadType(string arg)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == arg);
        }
    }
}