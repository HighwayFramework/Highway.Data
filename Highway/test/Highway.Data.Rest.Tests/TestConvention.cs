using System;
using Highway.Data.Rest.Configuration.Conventions;

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

        public string DefaultFormat()
        {
            return "{{{1}}}/{0}";
        }
    }
}