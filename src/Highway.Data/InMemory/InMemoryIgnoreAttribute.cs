using System;

namespace Highway.Data.Contexts
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InMemoryIgnoreAttribute : Attribute
    {
    }
}
