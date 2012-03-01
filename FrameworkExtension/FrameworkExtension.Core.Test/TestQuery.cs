using System.Collections.Generic;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.QueryObjects;

namespace FrameworkExtension.Core.Test
{
    public class TestQuery : QueryObject<Foo>
    {
        public TestQuery()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}