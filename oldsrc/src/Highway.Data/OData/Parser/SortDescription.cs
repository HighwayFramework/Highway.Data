using System;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser
{
    /// <summary>
    ///     Defines a sort description.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> to sort.</typeparam>
    public class SortDescription<T>
    {
        private readonly SortDirection _direction;
        private readonly Expression _keySelector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SortDescription{T}" /> class.
        /// </summary>
        /// <param name="keySelector">The function to select the sort key.</param>
        /// <param name="direction">The sort direction.</param>
        public SortDescription(Expression keySelector, SortDirection direction)
        {
            _keySelector = keySelector;
            _direction = direction;
        }

        /// <summary>
        ///     Gets the sort direction.
        /// </summary>
        public SortDirection Direction
        {
            get { return _direction; }
        }

        /// <summary>
        ///     Gets the key to sort by.
        /// </summary>
        public Expression KeySelector
        {
            get { return _keySelector; }
        }
    }
}