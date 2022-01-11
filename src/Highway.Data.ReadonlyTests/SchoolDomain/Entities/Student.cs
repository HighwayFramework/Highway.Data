using System;

namespace Highway.Data.ReadonlyTests
{
    public class Student
    {
        public DateTime? DoB { get; set; }

        public Grade Grade { get; set; }

        public decimal Height { get; set; }

        public string Name { get; set; }

        public int StudentID { get; set; }

        public float Weight { get; set; }
    }
}
