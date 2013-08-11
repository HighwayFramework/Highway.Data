using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.Rest.Configuration.Interfaces
{
    public interface IRestTypeDefinition
    {
        string SingleUri { get; }
        string AllUri { get; }
        Type ConfiguredType { get; }
        PropertyInfo KeyProperty { get; } 
    }
}   