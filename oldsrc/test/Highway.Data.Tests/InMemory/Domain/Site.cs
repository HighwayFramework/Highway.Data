using System;

namespace Highway.Data.Tests.InMemory.Domain
{
    public class Site
    {
        public Blog Blog { get; set; }
        public int Id { get; set; }
    }

    public class IdentifiablePerson<T> : IIdentifiable<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}