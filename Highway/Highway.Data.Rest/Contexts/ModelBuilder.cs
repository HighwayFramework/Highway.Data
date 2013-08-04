using Highway.Data.Rest.Configuration.Conventions;
using Highway.Data.Rest.Configuration.Entities;

namespace Highway.Data.Rest.Contexts
{
    public class ModelBuilder
    {
        private readonly IRestConvention _convention;

        public ModelBuilder() : this(new DefaultConvention()) { }
        public ModelBuilder(IRestConvention convention)
        {
            _convention = convention;
        }

        public ModelDefinitions Compile()
        {
            throw new System.NotImplementedException();
        }

        public RestTypeConfiguration<T> Entity<T>()
        {
            return new RestTypeConfiguration<T>(_convention);
        }
    }
}