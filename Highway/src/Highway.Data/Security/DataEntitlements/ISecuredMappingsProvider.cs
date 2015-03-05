using System.Collections.Generic;

namespace Highway.Data.Security.DataEntitlements
{
    public interface ISecuredMappingsProvider
    {
        IEnumerable<ISecuredRelationshipBuilder> GetMappings();
    }
}