using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Linq.Expressions;

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
    }
}