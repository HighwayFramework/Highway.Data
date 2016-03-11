using System.Collections.Generic;

namespace Highway.Data.Tests.Security.ComplexDomain
{
    public class Blog : IIdentifiable<long>
    {
        public long Id { get; set; }
        public ICollection<Post> Posts { get; set; }

        public Author Author { get; set; }
        
    }
}