using System;
namespace Highway.Data.Tests.InMemory.Domain
{
    public class Post
    {
        public Post()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}