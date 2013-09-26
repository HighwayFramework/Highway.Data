using System.Collections.Generic;

namespace Highway.Data.Tests.TestDomain
{
    public class Baz
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Qux> Quxes { get; set; }
    }
}