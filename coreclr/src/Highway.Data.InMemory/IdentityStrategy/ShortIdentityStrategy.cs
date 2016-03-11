using System;
using System.Linq.Expressions;

namespace Highway.Data.InMemory
{
    public class ShortIdentityStrategy<T> : IdentityStrategy<T, short>
        where T : class
    {
        static Object lockObject = new Object();

        static ShortIdentityStrategy()
        {
            Generator = GenerateShort;
        }

        public ShortIdentityStrategy(Expression<Func<T, short>> property)
            : base(property)
        {
        }

        private static short GenerateShort()
        {
            lock (lockObject) { return ++LastValue; }
        }
        protected override bool IsDefaultUnsetValue(short id)
        {
            return id == 0;
        }
    }
}
