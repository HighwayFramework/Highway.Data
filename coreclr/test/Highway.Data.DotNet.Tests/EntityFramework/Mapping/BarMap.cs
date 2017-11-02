
using System.Data.Entity.ModelConfiguration;
using Highway.Data.Tests.TestDomain;


namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class BarMap : EntityTypeConfiguration<Bar>
    {
        public BarMap()
        {
            ToTable("Bars");
            HasKey(x => x.Id);
            Property(x => x.Name).IsOptional();
        }
    }
}