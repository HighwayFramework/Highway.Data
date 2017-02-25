namespace Highway.Data.Tests.Entities
{
    public class ExampleRoot : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
