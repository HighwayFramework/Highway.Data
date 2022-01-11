using System.Data.Entity;

namespace Highway.Data.ReadonlyTests.SchoolDomain
{
    public class SchoolMapping : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GradeMap());
            modelBuilder.Configurations.Add(new StudentMap());
        }
    }
}
