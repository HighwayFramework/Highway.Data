using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    /// <summary>
    ///     Custom value expression factory.
    /// </summary>
    public interface IValueExpressionFactory
    {
        /// <summary>
        ///     Returns a value indicating whether the factory can handle a give <see cref="Type" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> to check.</param>
        /// <returns><code>true</code> if the <see cref="Type" /> can be handled, otherwise <code>false</code>.</returns>
        bool Handles(Type type);

        /// <summary>
        ///     Converts the passed OData style value to an <see cref="Expression" />.
        /// </summary>
        /// <param name="token">The value token to convert.</param>
        /// <returns>The value as a <see cref="ConstantExpression" />.</returns>
        ConstantExpression Convert(string token);
    }
}