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
    public class Given_A_Context_Without_Mappings
    {
        private DataContext context;
        private FooMappingConfiguration mapping;

        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer(new EntityFrameworkIntializer());
            mapping = new FooMappingConfiguration();
            context = new TestDataContext(
                connectionString: "Data Source=(localDb);Initial Catalog=Highway.Data.Test.Db;Integrated Security=True",
                mapping: mapping,
                logger: new NoOpLogger());
        }

        [TestMethod]
        public void Mappings_Can_Be_Injected_instead_of_explicitly_coded_in_the_context()
        {
            //Arrange

            //Act
            try
            {
                context.Set<Foo>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Suppress the error from the context. This allows us to test the mappings peice without having to actually map.
            }


            //Assert
            mapping.Called.Should().Be(true);
        }

        //[TestMethod]
        //public void Should_Apply_Context_Configuration_On_Construction()
        //{
        //    //Arrange
        //    var mockConfig = MockRepository.GenerateStrictMock<IContextConfiguration>();
        //    mockConfig.Expect(x => x.ConfigureContext(Arg<DbContext>.Is.Anything)).Repeat.Once();

        //    //Act
        //    var target = new DataContext("Test", new FooMappingConfiguration(), mockConfig);

        //    //Assert
        //    mockConfig.VerifyAllExpectations();
        //}
    }
}
