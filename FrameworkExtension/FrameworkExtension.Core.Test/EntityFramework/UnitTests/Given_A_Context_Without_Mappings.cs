using System;
using System.Data.Entity;
using System.Linq;
using FrameworkExtension.Core.Contexts;
using FrameworkExtension.Core.Mappings;
using FrameworkExtension.Core.Test.Properties;
using FrameworkExtension.Core.Test.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FrameworkExtension.Core.Test.EntityFramework.UnitTests
{
    [TestClass]
    public class Given_A_Context_Without_Mappings
    {
        [TestMethod]
        public void Mappings_Can_Be_Injected_instead_of_explicitly_coded_in_the_context()
        {
            //Arrange
            var mappings = MockRepository.GenerateMock<MappingConfiguration>();
            mappings.Expect(x => x.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything));

            //Act
            var target = new EntityFrameworkContext(Settings.Default.Connection, mappings);
            try
            {
                target.Commit();
            }
            catch (Exception)
            {
                //Suppress the error from the context. This allows us to test the mappings peice without having to actually map.
            }
            

            //Assert
            mappings.VerifyAllExpectations();
        }

    }
}