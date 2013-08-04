using System;

namespace Highway.Data.Rest.Configuration.Conventions
{
    public interface IRestConvention
    {
        string DefaultRoute(Type type);

        string DefaultKey(Type type);

        string DefaultFormat();
    }
}