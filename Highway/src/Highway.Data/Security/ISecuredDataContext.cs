using Highway.Data.Security.DataEntitlements;

namespace Highway.Data.Security
{
    internal interface ISecuredDataContext
    {
        SecuredMappingsCache MappingsCache { get; }

        IEntitlementProvider EntitlementProvider { get; }
    }
}
