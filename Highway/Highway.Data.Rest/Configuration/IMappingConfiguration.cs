using Highway.Data.Rest.Contexts;

namespace Highway.Data.Rest.Configuration
{
    public interface IMappingConfiguration
    {
        ModelBuilder Build(ModelBuilder modelBuilder);
    }
}