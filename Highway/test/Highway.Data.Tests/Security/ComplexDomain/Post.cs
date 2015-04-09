namespace Highway.Data.Tests.Security.ComplexDomain
{
    public class Post : IIdentifiable<long>
    {
        public long Id { get; set; }

        public Author Author { get; set; }
    }
}