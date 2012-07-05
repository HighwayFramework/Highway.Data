using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EntityFramework.Mappings;
using System.Data.Entity;

namespace Templates.Models
{
    public class Mappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            // TODO Create Mappings Here!
            modelBuilder.Entity<DeleteMe>();
        }
    }

    // TODO Delete this class once you've created your mappings
    public class DeleteMe
    {
        public int Id { get; set; }
    }
}