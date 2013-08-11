using System;

namespace Highway.Data.Rest.Configuration.Conventions
{
    public class DefaultConvention : IRestConvention
    {
        public virtual string DefaultRoute(Type type)
        {
            return type.Name.Pluralize().ToLowerInvariant();
        }

        public virtual string DefaultKey(Type type)
        {
            return "id";
        }

        public virtual RestActionFormat DefaultFormat()
        {
            return new RestActionFormat()
            {
                Single = "{0}/{{{1}}}",
                All = "{0}"
            };
        }
    }
}