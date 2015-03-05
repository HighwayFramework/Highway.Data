using System;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.OData
{
    public class UntypedCountedQueryable<T> : UntypedQueryable<T>
    {
        public UntypedCountedQueryable(IQueryable<T> source, Expression<Func<T, object>> projection, int inlineCount)
            : base(source, projection)
        {
            InlineCount = inlineCount;
        }

        public int InlineCount { get; private set; }
    }
}