using Highway.Data.Tests.TestDomain;
using System;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class EntityFrameworkIntializer : DropCreateInitializer<TestDataContext>
    {
        public EntityFrameworkIntializer() : base(SeedDatabase)
        {
        }

        private static void SeedDatabase(TestDataContext context)
        {
            for (int i = 0; i < 5; i++)
            {
                context.Add(new Foo());
            }
            context.SaveChanges();
        }
    }
}
