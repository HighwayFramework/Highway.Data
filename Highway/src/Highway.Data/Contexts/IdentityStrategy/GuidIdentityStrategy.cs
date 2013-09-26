using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Highway.Data.Contexts
{
    public class GuidIdentityStrategy<T> : IdentityStrategy<T, Guid>
            where T : class
    {
        static GuidIdentityStrategy()
        {
            Generator = Guid.NewGuid;
        }
    }
}
