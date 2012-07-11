using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.NHibernate.Tests.Mapping
{
    public class BarMap : EntityTypeConfiguration<Bar>
    {
        public BarMap()
        {
            this.ToTable("Bars");
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Name).IsOptional();
        }
    }
}