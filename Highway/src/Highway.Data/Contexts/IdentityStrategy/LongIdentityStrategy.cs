#region

using System;
using System.Linq.Expressions;
using System.Threading;

#endregion

namespace Highway.Data.Contexts
{
    public class LongIdentityStrategy<T> : IdentityStrategy<T, long>
        where T : class
    {
        static LongIdentityStrategy()
        {
            Generator = GenerateLong;
        }

        public LongIdentityStrategy(Expression<Func<T, long>> property)
            : base(property)
        {
        }

        private static long GenerateLong()
        {
            return Interlocked.Increment(ref LastValue);
        }
        protected override bool IsDefaultUnsetValue(long id)
        {
            return id == 0;
        }
    }
}