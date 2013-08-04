using Highway.Data.Rest.Configuration.Conventions;

namespace Highway.Data.Rest.ExampleAPI.Tests
{
    public class OverrideTestConvention : DefaultConvention
    {
        public override string DefaultFormat()
        {
            return "{{{1}}}/{1}";
        }
    }
}