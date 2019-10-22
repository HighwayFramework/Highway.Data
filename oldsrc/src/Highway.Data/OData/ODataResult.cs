using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Highway.Data.OData
{
    /// <summary>
    /// The object represents a JSON and XML serializable OData result.
    /// </summary>
    /// <typeparam name="T">The type of the resulting objects</typeparam>
    public class ODataResult<T>
    {
        /// <summary>
        /// Creates an OData result for the provided Queryable
        /// </summary>
        /// <param name="sourceQueryable">The queryable to wrap into the OData results</param>
        public ODataResult(UntypedQueryable<T> sourceQueryable)
        {
            _results = sourceQueryable.ToList();
            var queryable = sourceQueryable as UntypedCountedQueryable<T>;
            if (queryable != null)
            {
                _count = queryable.InlineCount;
            }
        }

        /// <summary>
        /// The objects returned by the query after OData filters, paging, sorting, and projection were applied
        /// </summary>
        public List<object> Results
        {
            get { return _results; }
        }

        /// <summary>
        /// The count of the objects in the query after Odata filtering was applied
        /// </summary>
        public int? Count
        {
            get { return _count; }
        }

        private readonly List<object> _results;

        private readonly int? _count;
    }
}