using System;
using System.Collections.Generic;

namespace Highway.Data.Filtering
{
    public static class CriteriaBuilder
    {
        private static string _on = "=";
        private static string _onOrBefore = "<=";
        private static string _onOrAfter = ">=";
        private static string _before = "<";
        private static string _after = ">";
        private static string _equal = "=";
        private static string _greaterThan = ">";
        private static string _greaterThanOrEqual = ">=";
        private static string _lessThan = "<";
        private static string _lessThanOrEqual = "<=";

        internal static BasicCriteria<T> Create<T>(string fieldName, string operation, T value)
        {
            return new BasicCriteria<T>(fieldName, operation, value);
        }

        public static Criteria On(this FieldIdentifier<DateTime> fieldIdentifier, DateTime value)
        {
            return Create(fieldIdentifier.FieldName, _on, value);
        }

        public static Criteria OnOrBefore(this FieldIdentifier<DateTime> fieldIdentifier, DateTime value)
        {
            return Create(fieldIdentifier.FieldName, _onOrBefore, value);
        }

        public static Criteria OnOrAfter(this FieldIdentifier<DateTime> fieldIdentifier, DateTime value)
        {
            return Create(fieldIdentifier.FieldName, _onOrAfter, value);
        }

        public static Criteria After(this FieldIdentifier<DateTime> fieldIdentifier, DateTime value)
        {
            return Create(fieldIdentifier.FieldName, _after, value);
        }

        public static Criteria Before(this FieldIdentifier<DateTime> fieldIdentifier, DateTime value)
        {
            return Create(fieldIdentifier.FieldName, _before, value);
        }

        public static Criteria IsEqualTo<T>(this FieldIdentifier<T> fieldIdentifier, T value)
        {
            return Create(fieldIdentifier.FieldName, _equal, value);
        }

        public static Criteria GreaterThan<T>(this FieldIdentifier<T> fieldIdentifier, T value)
        {
            return Create(fieldIdentifier.FieldName, _greaterThan, value);
        }

        public static Criteria LessThan<T>(this FieldIdentifier<T> fieldIdentifier, T value)
        {
            return Create(fieldIdentifier.FieldName, _lessThan, value);
        }

        public static Criteria GreaterThanOrEqual<T>(this FieldIdentifier<T> fieldIdentifier, T value)
        {
            return Create(fieldIdentifier.FieldName, _greaterThanOrEqual, value);
        }

        public static Criteria LessThanOrEqual<T>(this FieldIdentifier<T> fieldIdentifier, T value)
        {
            return Create(fieldIdentifier.FieldName, _lessThanOrEqual, value);
        }

        public static Criteria ByOperator<T>(this FieldIdentifier<T> fieldIdentifier, string @operator, T value)
        {
            if (_operators.ContainsKey(@operator)) @operator = _operators[@operator];
            return Create(fieldIdentifier.FieldName, @operator, value);
        }

        private static readonly Dictionary<string, string> _operators = new Dictionary<string, string>(){
                {"equal", "=" },
                {"greater than", ">" },
                {"greater than or equal", ">=" },
                {"less than", "<" },
                {"less than or equal", "<=" },
                {"on", "=" },
                {"after", ">" },
                {"on or after", ">=" },
                {"before", "<" },
                {"on or before", "<=" },
            };
    }
}