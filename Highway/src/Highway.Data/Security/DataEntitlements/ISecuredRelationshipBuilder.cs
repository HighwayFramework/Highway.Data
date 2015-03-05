using System.Collections.Generic;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Security.DataEntitlements
{
    /// <summary>
    /// Exposes the ability for implementers to build secured relationships.
    /// </summary>
    public interface ISecuredRelationshipBuilder
    {
        /// <summary>
        /// Builds the list of secured relationships for the implementing Type.
        /// </summary>
        /// <returns>The list of secured relationships for the implementing Type.</returns>
        IEnumerable<SecuredRelationship> BuildSecuredRelationships();
    }
}
