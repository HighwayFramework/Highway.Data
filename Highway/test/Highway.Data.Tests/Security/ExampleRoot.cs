namespace Highway.Data.Tests.Security
{
    public class ExampleRoot : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ExampleRoot NestedRoot { get; set; }
    }
}