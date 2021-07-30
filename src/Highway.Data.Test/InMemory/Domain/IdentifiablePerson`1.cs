using System;

namespace Highway.Data.Tests.InMemory.Domain
{
    public class IdentifiablePerson<T> : IIdentifiable<T>
        where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}
