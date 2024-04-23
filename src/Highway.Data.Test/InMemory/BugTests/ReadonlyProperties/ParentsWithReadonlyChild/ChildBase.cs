namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithReadonlyChild
{
    public class ChildBase : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
