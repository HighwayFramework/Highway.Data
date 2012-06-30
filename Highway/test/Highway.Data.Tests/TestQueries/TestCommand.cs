using Highway.Data.QueryObjects;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.Tests.TestQueries
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