using System.Collections.Generic;

namespace Highway.Data.Tests.Security
{
    public class ExampleLeaf : IIdentifiable<long>
    {
        public ExampleLeaf()
        {
            SecuredRoots = new List<ExampleRoot>();
        }

        public long Id { get; set; }

        public ExampleRoot SecuredRoot { get; set; }

        public ICollection<ExampleRoot> SecuredRoots { get; set; }

        public ICollection<ExampleLeaf> ReleatedLeaves { get; set; }
    }
}