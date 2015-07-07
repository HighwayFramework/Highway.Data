using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.OData
{
    public class UntypedQueryable<T> : IQueryable<object>
    {
        public UntypedQueryable(IQueryable<T> source, Expression<Func<T, object>> selectExpression)
        {
            _source = selectExpression == null ?
                (IQueryable)source :
                source.Select(selectExpression);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return Enumerable.Cast<object>(_source).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public Expression Expression
        {
            get
            {
                return _source.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _source.Provider;
            }
        }

        private readonly IQueryable _source;
    }
}