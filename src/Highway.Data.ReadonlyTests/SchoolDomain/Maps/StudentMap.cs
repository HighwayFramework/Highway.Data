using System.Data.Entity.ModelConfiguration;

namespace Highway.Data.ReadonlyTests
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            ToTable("Student");
            HasKey(x => x.StudentID);
        }
    }
}
