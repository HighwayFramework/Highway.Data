using System.Collections.Generic;

namespace Highway.Data.Security.DataEntitlements
{
    public interface IMappingsProvider
    {
        IEnumerable<ISecuredRelationshipBuilder> GetMappings();
    }
}