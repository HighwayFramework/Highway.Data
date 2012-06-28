using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using FrameworkExtension.Core.Test.TestDomain;

namespace FrameworkExtension.EntityFramework.Tests.Mapping
{
    public class FooMap : EntityTypeConfiguration<Foo>
    {
        public FooMap()
        {
            this.ToTable("Foos");
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Name).IsOptional();
        }
    }
}