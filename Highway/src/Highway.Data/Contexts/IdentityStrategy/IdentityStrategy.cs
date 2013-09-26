using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Highway.Data.Contexts
{
    public abstract class IdentityStrategy<TType, TIdentity>
            where TType : class
    {
        public static TIdentity LastValue = default(TIdentity);
        public static Func<TIdentity> Generator = null;

        public TIdentity Next()
        {
            if (Generator == null) throw new NotImplementedException();
            return Generator.Invoke();
        }
    }
}
