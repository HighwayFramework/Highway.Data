using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.StructureMap.Example.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.EntityFramework.StructureMap.Example
{
    public class HighwayDataMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
        }
    }
}
