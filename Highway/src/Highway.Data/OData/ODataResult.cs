using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Highway.Data.OData
{
    public class ODataResult<T>
    {
        private readonly List<object> _results;
        private int? _count;

        public ODataResult(UntypedQueryable<T> sourceQueryable)
        {
            _results = sourceQueryable.ToList();
            var queryable = sourceQueryable as UntypedCountedQueryable<T>;
            if (queryable != null)
            {
                _count = queryable.InlineCount;
            }
        }

        public dynamic GetResults()
        {
            dynamic result;
            if (_count.HasValue)
            {
                result = new
                {
                    Count = _count.Value,
                    Results = _results
                };
            }
            else
            {
                result = _results;
            }
            return result;
        }
    }
}