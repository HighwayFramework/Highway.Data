using System.Data.Entity.ModelConfiguration;

namespace Highway.Data.ReadonlyTests
{
    public class GradeMap : EntityTypeConfiguration<Grade>
    {
        public GradeMap()
        {
            ToTable("Grade");
            HasKey(x => x.GradeId);
        }
    }
}
