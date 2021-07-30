using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class EntityFrameworkInitializer : DropCreateInitializer<TestDataContext>
    {
        public EntityFrameworkInitializer()
            : base(SeedDatabase)
        {
        }

        private static void SeedDatabase(TestDataContext context)
        {
            for (var i = 0; i < 5; i++)
            {
                context.Add(new Foo());
            }

            context.SaveChanges();
        }
    }
}
