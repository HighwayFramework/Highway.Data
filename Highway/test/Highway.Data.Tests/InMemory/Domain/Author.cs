using System;
namespace Highway.Data.Tests.InMemory.Domain
{
    public class Author
    {
        public Author()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}