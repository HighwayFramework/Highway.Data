using System.Data.Entity;

namespace Highway.Data.GettingStarted
{
    public class GettingStartedMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            //Approach 1
            modelBuilder.Entity<Person>().HasKey(x=>x.Id).ToTable("People");

            //Approach 2
            modelBuilder.Configurations.Add(new AccountMap());
        }
    }

   
}
