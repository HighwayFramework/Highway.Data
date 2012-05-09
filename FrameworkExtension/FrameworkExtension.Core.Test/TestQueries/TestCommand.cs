using FrameworkExtension.Core.QueryObjects;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.Core.Test.TestQueries
{
    public class TestCommand : Command
    {
        public bool Called { get; set; }

        public TestCommand()
        {
            ContextQuery = db =>
                               {
                                   db.AsQueryable<Foo>();
                                   Called = true;
                               };
        }
    }
}