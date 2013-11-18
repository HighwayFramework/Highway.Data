#region

using Highway.Data.Rest.Contexts;

#endregion

namespace Highway.Data.Rest.Configuration
{
    public interface IMappingConfiguration
    {
        ModelBuilder Build(ModelBuilder modelBuilder);
    }
}