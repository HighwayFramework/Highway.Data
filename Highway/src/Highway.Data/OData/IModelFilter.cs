using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.OData.Parser;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Defines the public interface for a model filter.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> of item to filter.</typeparam>
    public interface IModelFilter<T>
    {
        /// <summary>
        ///     Gets the filter expression.
        /// </summary>
        Expression<Func<T, bool>> FilterExpression { get; }

        /// <summary>
        ///     Gets the projection expression for the odata query
        /// </summary>
        Expression<Func<T, object>> ProjectExpression { get; }

        /// <summary>
        ///     Gets the amount of items to take.
        /// </summary>
        int TakeCount { get; }

        /// <summary>
        ///     Gets the amount of items to skip.
        /// </summary>
        int SkipCount { get; }

        /// <summary>
        ///     Gets the <see cref="SortDescription{T}" /> for the sequence.
        /// </summary>
        IEnumerable<SortDescription<T>> SortDescriptions { get; }

        /// <summary>
        ///     True if the inline count was asked for as part of the OData query
        /// </summary>
        bool IncludeCount { get; }

        /// <summary>
        ///     Filters the passed collection with the defined filter.
        /// </summary>
        /// <param name="source">The source items to filter.</param>
        /// <returns>A filtered enumeration of the source items.</returns>
        IQueryable<T> Filter(IEnumerable<T> source);

        /// <summary>
        ///     Projects the passed collection with the defined selector.
        /// </summary>
        /// <param name="source">The source query to project.</param>
        /// <returns>A projected enumeration source query.</returns>
        UntypedQueryable<T> Project(IQueryable<T> source);
    }
}