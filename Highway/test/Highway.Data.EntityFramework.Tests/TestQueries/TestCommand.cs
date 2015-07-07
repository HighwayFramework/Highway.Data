
using Highway.Data.Tests.TestDomain;


namespace Highway.Data.EntityFramework.Tests.TestQueries
{
    public class TestCommand : Command
    {
        public TestCommand()
        {
            ContextQuery = db =>
            {
                db.AsQueryable<Foo>();
                Called = true;
            };
        }

        public bool Called { get; set; }
    }
}