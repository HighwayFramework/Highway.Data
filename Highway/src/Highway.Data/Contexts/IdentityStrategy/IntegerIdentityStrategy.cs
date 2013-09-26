using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private static int GenerateInt()
        {
            return Interlocked.Increment(ref LastValue);
        }
    }
}