using System;
using System.Collections.Generic;

namespace Highway.Data.OData.Parser
{
    internal static class FunctionExtensions
    {
        private static readonly Dictionary<string, Type> KnownFunctions = new Dictionary<string, Type>
        {
            {"length", typeof (int)},
            {"substring", typeof (string)},
            {"substringof", typeof (bool)},
            {"endswith", typeof (bool)},
            {"startswith", typeof (bool)},
            {"indexof", typeof (int)},
            {"tolower", typeof (string)},
            {"toupper", typeof (string)},
            {"trim", typeof (string)},
            {"year", typeof (int)},
            {"month", typeof (int)},
            {"day", typeof (int)},
            {"hour", typeof (int)},
            {"minute", typeof (int)},
            {"second", typeof (int)},
            {"floor", typeof (int)},
            {"ceiling", typeof (int)},
            {"round", typeof (double)}
        };

        public static Type GetFunctionType(this string functionName)
        {
            return KnownFunctions[functionName];
        }
    }
}