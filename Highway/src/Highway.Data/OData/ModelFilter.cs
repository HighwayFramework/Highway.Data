using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.OData.Parser;

namespace Highway.Data.OData
{
    internal class ModelFilter<T> : IModelFilter<T>
    {
        private readonly Expression<Func<T, bool>> _filterExpression;
        private readonly bool _includeCount;
        private readonly Expression<Func<T, object>> _projectExpression;
        private readonly int _skip;
        private readonly IEnumerable<SortDescription<T>> _sortDescriptions;
        private readonly int _top;

        public ModelFilter(Expression<Func<T, bool>> filterExpression, Expression<Func<T, object>> projectExpression,
            IEnumerable<SortDescription<T>> sortDescriptions, int skip, int top, bool includeCount)
        {
            _skip = skip;
            _top = top;
            _includeCount = includeCount;
            _filterExpression = filterExpression;
            _projectExpression = projectExpression;
            _sortDescriptions = sortDescriptions ?? Enumerable.Empty<SortDescription<T>>();
        }

        public Expression<Func<T, object>> ProjectExpression
        {
            get { return _projectExpression; }
        }

        /// <summary>
        ///     Gets the amount of items to take.
        /// </summary>
        public int TakeCount
        {
            get { return _top; }
        }

        /// <summary>
        ///     Gets the filter expression.
        /// </summary>
        public Expression<Func<T, bool>> FilterExpression
        {
            get { return _filterExpression; }
        }

        /// <summary>
        ///     Gets the amount of items to skip.
        /// </summary>
        public int SkipCount
        {
            get { return _skip; }
        }

        /// <summary>
        ///     Gets the <see cref="SortDescription{T}" /> for the sequence.
        /// </summary>
        public IEnumerable<SortDescription<T>> SortDescriptions
        {
            get { return _sortDescriptions; }
        }

        public bool IncludeCount
        {
            get { return _includeCount; }
        }

        public IQueryable<T> Filter(IEnumerable<T> model)
        {
            var result = _filterExpression != null
                ? model.AsQueryable().Where(_filterExpression)
                : model.AsQueryable();

            if (_sortDescriptions.Any())
            {
                var isFirst = true;
                foreach (var sortDescription in _sortDescriptions.Where(x => x != null))
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        result = sortDescription.Direction == SortDirection.Ascending
                            ? result.OrderBy(sortDescription.KeySelector)
                            : result.OrderByDescending(sortDescription.KeySelector);
                    }
                    else
                    {
                        var orderedEnumerable = (IOrderedQueryable<T>) result;

                        result = sortDescription.Direction == SortDirection.Ascending
                            ? orderedEnumerable.ThenBy(sortDescription.KeySelector)
                            : orderedEnumerable.ThenByDescending(sortDescription.KeySelector);
                    }
                }
            }

            if (_skip > 0)
            {
                result = result.Skip(_skip);
            }

            if (_top > -1)
            {
                result = result.Take(_top);
            }

            return result;
        }

        public IQueryable<object> Project(IQueryable<T> query)
        {
            return new UntypedQueryable<T>(query, _projectExpression);
        }
    }
}