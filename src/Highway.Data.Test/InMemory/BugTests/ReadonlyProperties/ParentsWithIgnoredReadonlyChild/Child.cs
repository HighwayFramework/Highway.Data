namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithIgnoredReadonlyChild
{
    public class Child : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
