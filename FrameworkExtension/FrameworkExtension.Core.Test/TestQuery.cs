using System.Collections.Generic;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.Test
{
    public class TestQuery : IQueryObject<Foo>
    {
        public IEnumerable<Foo> Execute(IDbContext context)
        {
            return context.AsQueryable<Foo>();
        }
    }
}