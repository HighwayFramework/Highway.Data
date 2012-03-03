using FrameworkExtension.Core.QueryObjects;
using FrameworkExtension.Core.Test.EntityFramework.UnitTests;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.Core.Test
{
    public class TestQuery : QueryObject<Foo>
    {
        public TestQuery()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}