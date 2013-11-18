#region

using System.Linq;
using Highway.Data.Tests.InMemory.Domain;

#endregion

namespace Highway.Data.Tests.InMemory.ScenarioTests.Queries
{
    public class FindBlogByTitle : Scalar<Blog>
    {
        public FindBlogByTitle(string title)
        {
            ContextQuery = c => c.AsQueryable<Blog>().Single(x => x.Title == title);
        }
    }
}