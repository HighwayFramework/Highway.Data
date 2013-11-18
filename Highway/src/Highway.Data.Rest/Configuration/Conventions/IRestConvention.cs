#region

using System;

#endregion

namespace Highway.Data.Rest.Configuration.Conventions
{
    public interface IRestConvention
    {
        string DefaultRoute(Type type);

        string DefaultKey(Type type);

        RestActionFormat DefaultFormat();
    }
}