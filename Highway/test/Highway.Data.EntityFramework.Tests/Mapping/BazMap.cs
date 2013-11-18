#region

using System.Data.Entity.ModelConfiguration;
using Highway.Data.Tests.TestDomain;

#endregion

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class BazMap : EntityTypeConfiguration<Baz>
    {
        public BazMap()
        {
            ToTable("Bazs");
            HasKey(x => x.Id);
            Property(x => x.Name).IsOptional();
            HasMany(x => x.Quxes).WithOptional();
        }
    }
}