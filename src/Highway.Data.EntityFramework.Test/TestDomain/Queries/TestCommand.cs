using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Test.TestDomain.Queries
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
