﻿using System;
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

        public string AttributeName { get { return string.Concat(DynamicFilterConstants.ATTRIBUTE_NAME_PREFIX, DynamicFilterConstants.DELIMETER, _CLRType.Name, DynamicFilterConstants.DELIMETER, FilterName); } }
        public string CreateDynamicFilterName(string parameterName)
        {
            return string.Concat(DynamicFilterConstants.PARAMETER_NAME_PREFIX, DynamicFilterConstants.DELIMETER, FilterName, DynamicFilterConstants.DELIMETER, parameterName);
        }

        public string CreateFilterDisabledParameterName()
        {
            return string.Concat(DynamicFilterConstants.PARAMETER_NAME_PREFIX, DynamicFilterConstants.DELIMETER, FilterName, DynamicFilterConstants.DELIMETER, DynamicFilterConstants.FILTER_DISABLED_NAME);
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
