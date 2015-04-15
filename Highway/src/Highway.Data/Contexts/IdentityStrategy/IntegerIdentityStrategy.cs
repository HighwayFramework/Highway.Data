
using System;
using System.Linq.Expressions;
using System.Threading;


namespace Highway.Data.Contexts
{
    public class IntegerIdentityStrategy<T> : IdentityStrategy<T, int>
        where T : class
    {
        static IntegerIdentityStrategy()
        {
            Generator = GenerateInt;
        }

        public IntegerIdentityStrategy(Expression<Func<T, int>> property)
            : base(property)
        {
        }

        private static int GenerateInt()
        {
            return Interlocked.Increment(ref LastValue);
        }

        protected override bool IsDefaultUnsetValue(int id)
        {
            return id == 0;
        }
    }
}