using System;
using System.Data.Entity;
using System.Linq;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.Tests;
using Castle.MicroKernel.Registration;
using Highway.Data.EntityFramework.Tests.Initializer;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Context_Without_Mappings : ContainerTest<DataContext>
    {
        private IMappingConfiguration _mockMapping;
        public override void RegisterComponents(Castle.Windsor.IWindsorContainer container)
        {
            _mockMapping = MockRepository.GenerateMock<IMappingConfiguration>();
            container.Register(Component.For<IMappingConfiguration>().Instance(_mockMapping));
            base.RegisterComponents(container);
        }

        public override DataContext ResolveTarget()
        {
            Database.SetInitializer(new EntityFrameworkIntializer());
            return Container.Resolve<DataContext>(new { connectionString = Settings.Default.Connection });
        }

        [TestMethod]
        public void Mappings_Can_Be_Injected_instead_of_explicitly_coded_in_the_context()
        {
            //Arrange
            _mockMapping.Expect(x => x.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything));

            //Act
            try
            {
                target.Set<Foo>().ToList();
            }
            catch (Exception)
            {
                //Suppress the error from the context. This allows us to test the mappings peice without having to actually map.
            }
            

            //Assert
            _mockMapping.VerifyAllExpectations();
        }
    }
}