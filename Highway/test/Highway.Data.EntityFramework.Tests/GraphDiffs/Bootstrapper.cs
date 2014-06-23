using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.GraphDiffs
{
    [TestClass]
    public class Bootstrapper
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TestDbContext>());
        }
    }
}
