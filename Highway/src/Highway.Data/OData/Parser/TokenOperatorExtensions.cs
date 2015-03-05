using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Highway.Data.OData.Parser
{
    internal static class TokenOperatorExtensions
    {
        private static readonly string[] Operations = {"eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not"};
        private static readonly string[] Combiners = {"and", "or", "not"};
        private static readonly string[] Arithmetic = {"add", "sub", "mul", "div", "mod"};
        private static readonly string[] BooleanFunctions = {"substringof", "endswith", "startswith"};

        private static readonly Regex CollectionFunctionRx = new Regex(@"^[0-9a-zA-Z_]+/(all|any)\((.+)\)$",
            RegexOptions.Compiled);

        private static readonly Regex CleanRx = new Regex(@"^\((.+)\)$", RegexOptions.Compiled);
        private static readonly Regex FunctionRegex = new Regex(@"^([^()/]+)\(.+\)$");
        private static readonly Regex StringStartRx = new Regex("^[(]*'", RegexOptions.Compiled);
        private static readonly Regex StringEndRx = new Regex("'[)]*$", RegexOptions.Compiled);

        public static bool IsCombinationOperation(this string operation)
        {
            return Combiners.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsOperation(this string operation)
        {
            return Operations.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsArithmetic(this string operation)
        {
            return Arithmetic.Any(x => string.Equals(x, operation, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsImpliedBoolean(this string expression)
        {
            if (!string.IsNullOrWhiteSpace(expression) && !expression.IsEnclosed() && expression.IsFunction())
            {
                var split = expression.Split(' ');
                return !split.Intersect(Operations).Any()
                       && !split.Intersect(Combiners).Any()
                       && (BooleanFunctions.Any(x => split[0].StartsWith(x, StringComparison.OrdinalIgnoreCase)) ||
                           CollectionFunctionRx.IsMatch(expression));
            }

            return false;
        }

        public static Match EnclosedMatch(this string expression)
        {
            return CleanRx.Match(expression);
        }

        public static bool IsEnclosed(this string expression)
        {
            var match = expression.EnclosedMatch();
            return match != null && match.Success;
        }

        public static bool IsStringStart(this string expression)
        {
            return !string.IsNullOrWhiteSpace(expression) && StringStartRx.IsMatch(expression);
        }

        public static bool IsStringEnd(this string expression)
        {
            return !string.IsNullOrWhiteSpace(expression) && StringEndRx.IsMatch(expression);
        }

        public static string GetFunctionName(this string expression)
        {
            var functionMatch = FunctionRegex.Match(expression);
            if (functionMatch.Success)
            {
                return functionMatch.Groups[1].Value;
            }

            return string.Empty;
        }

        public static bool IsFunction(this string expression)
        {
            var open = expression.IndexOf('(');
            var close = expression.IndexOf(')');

            return open > 0 && close > -1;
        }
    }
}