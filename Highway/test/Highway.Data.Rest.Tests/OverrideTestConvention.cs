#region

using Highway.Data.Rest.Configuration.Conventions;

#endregion

namespace Highway.Data.Rest.Tests
{
    public class OverrideTestConvention : DefaultConvention
    {
        public override RestActionFormat DefaultFormat()
        {
            return new RestActionFormat
            {
                Single = "{{{1}}}/{1}",
                All = "{{{1}}}"
            };
        }
    }
}