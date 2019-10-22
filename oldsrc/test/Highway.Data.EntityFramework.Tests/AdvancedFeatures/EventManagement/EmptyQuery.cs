using System.Collections.Generic;
using System.Linq;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    public class EmptyQuery : Query<Foo>
    {
        public EmptyQuery()
        {
            ContextQuery = c=> new List<Foo>().AsQueryable();
        }
    }
}