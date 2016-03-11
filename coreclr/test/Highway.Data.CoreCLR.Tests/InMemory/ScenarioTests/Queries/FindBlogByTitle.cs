using System.Linq;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;

namespace Highway.Data.CoreCLR.Tests.InMemory.ScenarioTests.Queries
{
    public class FindBlogByTitle : Scalar<Blog>
    {
        public FindBlogByTitle(string title)
        {
            ContextQuery = c => c.AsQueryable<Blog>().Single(x => x.Title == title);
        }
    }
}
