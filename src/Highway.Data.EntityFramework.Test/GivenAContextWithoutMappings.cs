using Common.Logging.Simple;
using FluentAssertions;

using Highway.Data.EntityFramework.Test.TestDomain;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Highway.Data.EntityFramework.Test
{
    [TestClass]
    public class GivenAContextWithoutMappings
    {
        private DataContext _context;
        private FooMappingConfiguration _mapping;

        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer(new EntityFrameworkInitializer());
            _mapping = new FooMappingConfiguration();
            _context = new TestDataContext(
                connectionString: "Data Source=(localDb);Initial Catalog=Highway.Data.Test.Db;Integrated Security=True;Connection Timeout=1",
                mapping: _mapping,
                logger: new NoOpLogger());
        }

        [TestMethod]
        public void Mappings_Can_Be_Injected_instead_of_explicitly_coded_in_the_context()
        {
            //Arrange

            //Act
            try
            {
                _context.Set<Foo>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Suppress the error from the context. This allows us to test the mappings piece without having to actually map.
            }


            //Assert
            _mapping.Called.Should().Be(true);
        }
    }
}
