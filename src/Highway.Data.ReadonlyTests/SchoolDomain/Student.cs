using System;

namespace Highway.Data.ReadonlyTests
{
    public class Student
    {
        public DateTime? DateOfBirth { get; set; }

        public Grade Grade { get; set; }

        public decimal Height { get; set; }

        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public float Weight { get; set; }
    }
}
