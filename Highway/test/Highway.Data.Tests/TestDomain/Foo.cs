using System;
using System.Collections.Generic;
using Highway.Data;

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

        public Bar Bar { get; set; }

        public ICollection<Bar> Bars { get; set; }



        public object Test()
        {
            throw new NotImplementedException();
        }
    }
}