using Highway.Data.QueryObjects;
using Highway.Data.Test.TestDomain;

namespace Highway.Data.Test.TestQueries
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