namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentWithReadonlyChild
{
    public class Child : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
