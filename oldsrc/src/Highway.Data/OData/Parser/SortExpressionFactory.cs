using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser
{
    /// <summary>
    ///     Defines the SortExpressionFactory´.
    /// </summary>
    public class SortExpressionFactory : ISortExpressionFactory
    {
        private readonly IMemberNameResolver _nameResolver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SortExpressionFactory" /> class.
        /// </summary>
        /// <param name="nameResolver">The <see cref="IMemberNameResolver" /> for name resolution.</param>
        public SortExpressionFactory(IMemberNameResolver nameResolver)
        {
            _nameResolver = nameResolver;
        }

        /// <summary>
        ///     Creates an enumeration of sort descriptions from its string representation.
        /// </summary>
        /// <param name="filter">The string representation of the sort descriptions.</param>
        /// <typeparam name="T">The <see cref="Type" /> of item to sort.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}" /> if the passed sort descriptions are valid, otherwise null.</returns>
        public IEnumerable<SortDescription<T>> Create<T>(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return new SortDescription<T>[0];
            }

            var parameterExpression = Expression.Parameter(typeof (T), "x");

            var sortTokens = filter.Split(',');
            return from sortToken in sortTokens
                select sortToken.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                into sort
                let property = GetPropertyExpression<T>(sort.First(), parameterExpression)
                where property != null
                let direction =
                    sort.ElementAtOrDefault(1) == "desc" ? SortDirection.Descending : SortDirection.Ascending
                select new SortDescription<T>(property, direction);
        }

        private Expression GetPropertyExpression<T>(string propertyToken, ParameterExpression parameter)
        {
            if (string.IsNullOrWhiteSpace(propertyToken))
            {
                return null;
            }

            var parentType = typeof (T);

            var propertyChain = propertyToken.Split('/');
            var result = _nameResolver.CreateMemberExpression(parameter, propertyChain, parentType, null);
            var propertyExpression = result.Item2;
            parentType = result.Item1;

            if (propertyExpression == null)
            {
                throw new FormatException(propertyToken + " is not recognized as a valid property");
            }

            var funcType = typeof (Func<,>).MakeGenericType(typeof (T), parentType);

            return Expression.Lambda(funcType, propertyExpression, parameter);
        }
    }
}