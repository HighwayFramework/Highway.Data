using System.Linq;
using FrameworkExtension.Core.QueryObjects;
using FrameworkExtension.Core.Test.EntityFramework.UnitTests;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.Core.Test
{
    public class ScalarTestQuery : ScalarObject<int>
    {
        public ScalarTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}