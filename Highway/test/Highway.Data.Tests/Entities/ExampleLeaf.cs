namespace Highway.Data.Tests.Entities
{
    public class ExampleLeaf : IIdentifiable<long>
    {
        public long Id { get; set; }

        public ExampleRoot SecuredRoot { get; set; }
    }
}