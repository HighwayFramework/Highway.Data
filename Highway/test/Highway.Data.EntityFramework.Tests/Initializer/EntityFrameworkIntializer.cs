using System.Data.Entity;
using Highway.Data.NHibernate.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;
using Highway.Data.EntityFramework.Tests.UnitTests;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class EntityFrameworkIntializer : DropCreateDatabaseAlways<TestDataContext>
    {
        protected override void Seed(TestDataContext context)
        {
            for (int i = 0; i < 5;i++ )
            {
                context.Add(new Foo());
            }
            context.SaveChanges();
        }
    }
}