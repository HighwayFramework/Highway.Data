using System.Collections.Generic;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public interface IProvideRelationships
    {
        IEnumerable<Relationship> GetRelationships();
    }
}