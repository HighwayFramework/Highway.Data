#region

using System;
using System.Collections.Generic;

#endregion

namespace Highway.Data.Tests.TestDomain
{
    public class Foo : IIdentifiable<int>
    {
        [StoredProcedureAttributes.Name("testName")]
        public virtual string Name { get; set; }

        [StoredProcedureAttributes.Name("testAddress")]
        public virtual string Address { get; set; }

        public Bar Bar { get; set; }

        public ICollection<Bar> Bars { get; set; }

        [StoredProcedureAttributes.Name("testID")]
        public virtual int Id { get; set; }


        public object Test()
        {
            throw new NotImplementedException();
        }
    }
}