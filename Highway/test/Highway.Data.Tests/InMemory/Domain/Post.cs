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
    }
}