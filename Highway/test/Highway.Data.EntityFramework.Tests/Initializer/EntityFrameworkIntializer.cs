using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;



namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class EntityFrameworkIntializer : DropCreateDatabaseAlways<TestDataDataContext>
    {
        protected override void Seed(TestDataDataContext dataDataContext)
        {
            for (int i = 0; i < 5;i++ )
            {
                dataDataContext.Add(new Foo());
            }
            dataDataContext.SaveChanges();
        }
    }
}