using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.OData
{
    public class UntypedQueryable<T> : IQueryable<object>
    {
        private readonly IQueryable _source;

        public UntypedQueryable(IQueryable<T> source, Expression<Func<T, object>> projection)
        {
            _source = projection == null
                ? (IQueryable) source
                : source.Select(projection);
        }

        public Expression Expression
        {
            get { return _source.Expression; }
        }

        public Type ElementType
        {
            get { return typeof (T); }
        }

        public IQueryProvider Provider
        {
            get { return _source.Provider; }
        }

        public IEnumerator<object> GetEnumerator()
        {
            return Enumerable.Cast<object>(_source).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}