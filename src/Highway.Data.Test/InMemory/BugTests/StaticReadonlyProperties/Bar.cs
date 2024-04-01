namespace Highway.Data.Test.InMemory.BugTests.StaticReadonlyProperties
{
    public class Bar : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
