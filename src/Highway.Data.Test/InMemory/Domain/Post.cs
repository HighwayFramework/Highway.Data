namespace Highway.Data.Tests.InMemory.Domain
{
    public class Post
    {
        public Blog Blog { get; set; }

        public string Body { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }
    }
}
