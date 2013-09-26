using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Highway.Data.Tests.InMemory.Domain
{
    public class Blog
    {
        public Blog()
        {
            Posts = new Collection<Post>();    
        }
        public Author Author { get; set; }

        public ICollection<Post> Posts { get; set; } 
    }
}