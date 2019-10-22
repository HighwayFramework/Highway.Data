using Highway.Data.Tests.TestDomain;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Highway.Data.EntityFramework.Test.TestDomain
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
