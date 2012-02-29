using System.Linq;
using FrameworkExtension.Core.QueryObjects;

namespace FrameworkExtension.Core.Test
{
    public class ScalarTestQuery : ScalarObject<int>
    {
        public ScalarTestQuery()
        {
            ContextQuery = db => db.AsQueryable<Foo>().Count();
        }
    }
}