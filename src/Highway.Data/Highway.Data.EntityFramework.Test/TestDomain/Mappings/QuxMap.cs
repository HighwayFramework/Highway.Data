using Highway.Data.Tests.TestDomain;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class QuxMap : EntityTypeConfiguration<Qux>
    {
        public QuxMap()
        {
            ToTable("Quxs");
            HasKey(x => x.Id);
            Property(x => x.Name).IsOptional();
        }
    }
}
