using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.Security
{
    internal interface ISecurityContext
    {
        MappingsCache MappingsCache { get; }

        IEntitlementProvider EntitlementProvider { get; }
    }
}
