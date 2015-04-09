using System;
using System.Linq.Expressions;

namespace Highway.Data.EntityFramework.DynamicFilters
{
    internal class DynamicFilterDefinition
    {
        public string FilterName { get; private set; }

        /// <summary>
        /// Set if the filter is a single column equality filter.  Null if filter is a Predicate (LambdaExpression)
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Set if the filter is a LambdaExpression.  Null if filter is a single column equality filter.
        /// </summary>
        public LambdaExpression Predicate { get; private set; }

        private Type _CLRType;

        public string AttributeName { get { return string.Concat(DynamicFilterConstants.AttributeNamePrefix, DynamicFilterConstants.Delimeter, _CLRType.Name, DynamicFilterConstants.Delimeter, FilterName); } }
        public string CreateDynamicFilterName(string parameterName)
        {
            return string.Concat(DynamicFilterConstants.ParameterNamePrefix, DynamicFilterConstants.Delimeter, FilterName, DynamicFilterConstants.Delimeter, parameterName);
        }

        public string CreateFilterDisabledParameterName()
        {
            return string.Concat(DynamicFilterConstants.ParameterNamePrefix, DynamicFilterConstants.Delimeter, FilterName, DynamicFilterConstants.Delimeter, DynamicFilterConstants.FilterDisabledName);
        }

        internal DynamicFilterDefinition(string filterName, LambdaExpression predicate, string columnName, Type clrType)
        {
            FilterName = filterName;
            Predicate = predicate;
            ColumnName = columnName;
            _CLRType = clrType;
        }
    }
}
