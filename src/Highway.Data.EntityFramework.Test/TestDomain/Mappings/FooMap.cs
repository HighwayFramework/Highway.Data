﻿using System.Data.Entity.ModelConfiguration;

using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class FooMap : EntityTypeConfiguration<Foo>
    {
        public FooMap()
        {
            ToTable("Foos");
            HasKey(x => x.Id);
            Property(x => x.Name).IsOptional();
        }
    }
}
