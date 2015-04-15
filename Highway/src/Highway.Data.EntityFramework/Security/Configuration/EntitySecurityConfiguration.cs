using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class EntitySecurityConfiguration<T> : IProvideRelationships
    {
        private readonly ICollection<IRelationshipBuilder> _relationships = new List<IRelationshipBuilder>();

        public IEnumerable<Relationship> GetRelationships()
        {
            return _relationships.Select(x => x.Build());
        }

        public IRootBuilder<T> Root()
        {
            var builder = new SecurableBuilder<T>();
            _relationships.Add(builder);
            return new SecurableBuilder<T>();
        }

        public IComplexRelationshipBuilder<T> By(Func<ISimplePathBuilder<T>, IComplexPathBuilder<T>> pathDescriptor)
        {
            var builder = new RelationshipBuilder<T>();
            _relationships.Add(builder);
            return builder.By(pathDescriptor);
        }
    }
}