#region

using System;
using Highway.Data.Rest.Configuration.Conventions;

#endregion

namespace Highway.Data.Rest.Tests
{
    public class TestConvention : IRestConvention
    {
        public string DefaultRoute(Type type)
        {
            return "Test";
        }

        public string DefaultKey(Type type)
        {
            return "Id";
        }

        public RestActionFormat DefaultFormat()
        {
            return new RestActionFormat
            {
                Single = "{{{1}}}/{0}",
                All = "{{{1}}}"
            };
        }
    }
}