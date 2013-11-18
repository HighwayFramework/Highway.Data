#region

using System.Collections.Generic;
using Highway.Data.Rest.Configuration.Conventions;
using Highway.Data.Rest.Configuration.Entities;
using Highway.Data.Rest.Configuration.Interfaces;

#endregion

namespace Highway.Data.Rest.Contexts
{
    public class ModelBuilder
    {
        private readonly IRestConvention _convention;
        private readonly List<IRestTypeDefinition> _types;

        public ModelBuilder() : this(new DefaultConvention())
        {
        }

        public ModelBuilder(IRestConvention convention)
        {
            _types = new List<IRestTypeDefinition>();
            _convention = convention;
        }

        public ModelDefinitions Compile()
        {
            var modelDefinition = new ModelDefinitions();
            foreach (var type in _types)
            {
                modelDefinition.Add(type);
            }
            return modelDefinition;
        }

        public RestTypeConfiguration<T> Entity<T>()
        {
            var typeConfig = new RestTypeConfiguration<T>(_convention);
            _types.Add(typeConfig);
            return typeConfig;
        }
    }
}