using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;



namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class EntityFrameworkIntializer : DropCreateDatabaseAlways<TestDataContext>
    {
        protected override void Seed(TestDataContext dataContext)
        {
            for (int i = 0; i < 5;i++ )
            {
                dataContext.Add(new Foo());
            }
            dataContext.SaveChanges();
        }
    }
}