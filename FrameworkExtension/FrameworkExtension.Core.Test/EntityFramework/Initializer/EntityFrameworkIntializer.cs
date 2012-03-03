using System.Data.Entity;
using FrameworkExtension.Core.Test.EntityFramework.UnitTests;
using FrameworkExtension.Core.Test.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrameworkExtension.Core.Test.EntityFramework.Initializer
{
    public class EntityFrameworkIntializer : IDatabaseInitializer<EFTestContext>
    {
        public void InitializeDatabase(EFTestContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand("alter database FEEFTest set offline with rollback immediate");
                context.Database.ExecuteSqlCommand("alter database FEEFTest set online");
                context.Database.ExecuteSqlCommand("drop database FEEFTest");
            }
            context.Database.Create();
            Seed(context);
        }

        protected void Seed(EFTestContext context)
        {
            for (int i = 0; i < 5;i++ )
            {
                context.Add(new Foo());
            }
            context.SaveChanges();
        }
    }
}