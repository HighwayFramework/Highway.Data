#region

using System.Collections.Generic;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;

#endregion

namespace Highway.Data.EntityFramework.Tests.Initializer
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
        }
    }
}