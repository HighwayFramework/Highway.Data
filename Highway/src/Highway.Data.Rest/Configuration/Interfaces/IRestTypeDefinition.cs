#region

using System;
using System.Reflection;

#endregion

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