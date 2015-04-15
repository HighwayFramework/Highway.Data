using System.Collections.Generic;
using Highway.Data.EntityFramework.Security.Configuration;

namespace Highway.Data.EntityFramework.Security.Interfaces
{
    /// <summary>
    ///     Exposes the ability for implementers to build secured relationships.
    /// </summary>
    public interface IBuildSecuredRelationships
    {
        /// <summary>
        ///     Builds the list of secured relationships for the implementing Type.
        /// </summary>
        /// <returns>The list of secured relationships for the implementing Type.</returns>
        IEnumerable<Relationship> BuildSecuredRelationships();
    }
}