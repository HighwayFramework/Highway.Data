using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.Mapping
{
    public class FooMap : EntityTypeConfiguration<Foo>
    {
        public FooMap()
        {
            ToTable("Foos");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsOptional().HasMaxLength(12);
        }
    }
}