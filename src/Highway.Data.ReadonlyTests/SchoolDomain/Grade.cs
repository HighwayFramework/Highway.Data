using System.Collections.Generic;

namespace Highway.Data.ReadonlyTests
{
    public class Grade
    {
        public int GradeId { get; set; }

        public string GradeName { get; set; }

        public string Section { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
