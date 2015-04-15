
using System;
using System.Linq.Expressions;
using System.Threading;


namespace Highway.Data.Contexts
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