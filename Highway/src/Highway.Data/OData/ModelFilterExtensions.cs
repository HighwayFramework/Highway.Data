using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Highway.Data.OData.Parser;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Defines extension methods for model filters.
    /// </summary>
    public static class ModelFilterExtensions
    {
        /// <summary>
        ///     Filters the source collection using the passed query parameters.
        /// </summary>
        /// <param name="source">The source items to filter.</param>
        /// <param name="query">The query parameters defining the filter.</param>
        /// <typeparam name="T">The <see cref="Type" /> of items in the source collection.</typeparam>
        /// <returns>A filtered enumeration of the source collection.</returns>
        public static IQueryable<T> Filter<T>(this IEnumerable<T> source, NameValueCollection query)
        {
            var parser = new ParameterParser<T>();

            return Filter(source, parser.Parse(query));
        }

        /// <summary>
        ///     Projects the source queryable using the passed query parameters.
        /// </summary>
        /// <param name="source">The source items to project.</param>
        /// <param name="query">The query parameters defining the projection.</param>
        /// <typeparam name="T">The <see cref="Type" /> of items in the source collection.</typeparam>
        /// <returns>A projected enumeration of the source collection.</returns>
        public static IQueryable<object> Project<T>(this IEnumerable<T> source, NameValueCollection query)
        {
            var parser = new ParameterParser<T>();

            return Project(source.AsQueryable(), parser.Parse(query));
        }

        /// <summary>
        ///     Filters the source collection using the passed query parameters.
        /// </summary>
        /// <param name="source">The source items to filter.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <typeparam name="T">The <see cref="Type" /> of items in the source collection.</typeparam>
        /// <returns>A filtered and projected enumeration of the source collection.</returns>
        public static IQueryable<T> Filter<T>(this IEnumerable<T> source, IModelFilter<T> filter)
        {
            return filter == null ? source.AsQueryable() : filter.Filter(source);
        }

        /// <summary>
        ///     Projects the source collection using the passed query parameters.
        /// </summary>
        /// <param name="source">The source items to filter.</param>
        /// <param name="filter">The projection to apply.</param>
        /// <typeparam name="T">The <see cref="Type" /> of items in the source collection.</typeparam>
        /// <returns>A projected enumeration of the source collection.</returns>
        public static IQueryable<object> Project<T>(this IQueryable<T> source, IModelFilter<T> filter)
        {
            return filter == null ? source.OfType<object>().AsQueryable() : filter.Project(source);
        }
    }
}