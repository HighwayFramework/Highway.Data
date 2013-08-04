using System;
using Highway.Data.PrebuiltQueries;

namespace Highway.Data.Tests.TestDomain
{
    public class Foo : IIdentifiable<int>
    {
        [StoredProcedureAttributes.Name("testID")]
        public virtual int Id { get; set; }

        [StoredProcedureAttributes.Name("testName")]
        public virtual string Name { get; set; }

        [StoredProcedureAttributes.Name("testAddress")]
        public virtual string Address { get; set; }

        public object Test()
        {
            throw new NotImplementedException();
        }
    }
}