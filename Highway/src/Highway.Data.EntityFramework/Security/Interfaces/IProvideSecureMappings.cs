using System.Collections.Generic;

namespace Highway.Data.EntityFramework.Security.Interfaces
{
    public interface IProvideSecureMappings
    {
        IEnumerable<IBuildSecuredRelationships> CreateMappings();
    }
}