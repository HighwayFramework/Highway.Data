using System;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
