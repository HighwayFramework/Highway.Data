namespace Highway.Data.Tests.Security
{
    public class ExampleLeaf : IIdentifiable<long>
    {
        public long Id { get; set; }

        public ExampleRoot SecuredRoot { get; set; }
    }
}