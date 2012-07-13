using System;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Context_Without_Mappings
    {
        [TestMethod]
        public void Mappings_Can_Be_Injected_instead_of_explicitly_coded_in_the_context()
        {
            //Arrange
            var mapping = MockRepository.GenerateMock<IMappingConfiguration>();
            var mappings = new IMappingConfiguration[]{mapping};
            mapping.Expect(x => x.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything));

            //Act
            var target = new Context(Settings.Default.Connection, mappings);
            try
            {
                target.Set<Foo>().ToList();
            }
            catch (Exception)
            {
                //Suppress the error from the context. This allows us to test the mappings peice without having to actually map.
            }
            

            //Assert
            mapping.VerifyAllExpectations();
        }

    }
}