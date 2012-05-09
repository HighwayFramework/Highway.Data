using FrameworkExtension.Core.QueryObjects;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.Core.Test.TestQueries
{
    public class TestQuery : Query<Foo>
    {
        public TestQuery()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}