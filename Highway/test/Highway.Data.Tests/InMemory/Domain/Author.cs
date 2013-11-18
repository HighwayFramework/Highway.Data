#region

using System;

#endregion

namespace Highway.Data.Tests.InMemory.Domain
{
    public class Author
    {
        public Author()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TwitterHandle { get; set; }
    }
}