using System;
using System.Data.Entity;

namespace Highway.Data.EntityFramework.Test.SqlLiteDomain
{
    public class SqlLiteDomainMappings : IMappingConfiguration
    {
        public SqlLiteDomainMappings()
        {
        }

        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(e => e.Id);
        }
    }
}
