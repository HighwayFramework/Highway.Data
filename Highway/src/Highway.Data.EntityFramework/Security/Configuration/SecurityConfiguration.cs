using System;
using System.Collections.Generic;
using Highway.Data.EntityFramework.Security.Interfaces;

namespace Highway.Data.EntityFramework.Security.Configuration
{
    public class SecurityConfiguration : IBuildSecuredRelationships
    {
        private readonly Dictionary<Type, IProvideRelationships> _configurations =
            new Dictionary<Type, IProvideRelationships>();

        public IEnumerable<Relationship> BuildSecuredRelationships()
        {
            var relationships = new List<Relationship>();
            foreach (var securedRelationships in _configurations)
            {
                relationships.AddRange(securedRelationships.Value.GetRelationships());
            }
            return relationships;
        }

        public EntitySecurityConfiguration<T> Secure<T>()
        {
            if (_configurations.ContainsKey(typeof (T)))
            {
                return (EntitySecurityConfiguration<T>) _configurations[typeof (T)];
            }
            var configuration = new EntitySecurityConfiguration<T>();
            _configurations.Add(typeof (T), configuration);
            return configuration;
        }
    }
}