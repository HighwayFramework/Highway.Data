using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.Rest.Configuration.Interfaces
{
    public interface IRestTypeDefinition
    {
        string Uri { get; }
        PropertyInfo KeyProperty { get; } 
    }
}   