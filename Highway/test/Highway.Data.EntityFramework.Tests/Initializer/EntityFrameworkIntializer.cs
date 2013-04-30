using System;
using System.Collections.Generic;
using System.Data.Entity;
using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class EntityFrameworkIntializer : DropCreateInitializer<TestDataContext>
    {
        private static string _GetFoos = @"Create PROCEDURE [dbo].[GetFoos]
	@Name varchar(20) 
AS
BEGIN
	SET NOCOUNT ON;

	Select x.Id as testID, x.Name as testName, x.Address as testAddress 
	from dbo.Foos as x
	where x.Name = @Name
END";

        public EntityFrameworkIntializer() : base(SeedDatabase, GetStoredProcedures)
        {
            
        }

        private static IEnumerable<string> GetStoredProcedures()
        {
            yield return _GetFoos;
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