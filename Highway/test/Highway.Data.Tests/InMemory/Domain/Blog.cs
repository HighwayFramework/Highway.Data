using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Highway.Data.Tests.InMemory.Domain
{
    public class Blog
    {
        public Blog()
        {
            Posts = new Collection<Post>();
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public Author Author { get; set; }

        public ICollection<Post> Posts { get; set; } 
    }
}