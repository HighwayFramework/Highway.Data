
using System;
using System.Data.Entity;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Highway.Data.EntityFramework.Tests.Initializer;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.Tests;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;


namespace Highway.Data.EntityFramework.Tests.Contexts
{
    [TestClass]
    public class Given_A_Context_Without_Mappings : ContainerTest<DataContext>
    {
        private IMappingConfiguration _mockMapping;

        public override void RegisterComponents(IWindsorContainer container)
        {
            _mockMapping = MockRepository.GenerateMock<IMappingConfiguration>();
            container.Register(Component.For<IMappingConfiguration>().Instance(_mockMapping));
            base.RegisterComponents(container);
        }

        public override DataContext ResolveTarget()
        {
            Database.SetInitializer(new EntityFrameworkIntializer());
            return Container.Resolve<DataContext>(new {connectionString = Settings.Default.Connection});
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

        [TestMethod]
        public void Should_Apply_Context_Configuration_On_Construction()
        {
            //Arrange
            var mockConfig = MockRepository.GenerateStrictMock<IContextConfiguration>();
            mockConfig.Expect(x => x.ConfigureContext(Arg<DbContext>.Is.Anything)).Repeat.Once();

            //Act
            var target = new DataContext("Test", new FooMappingConfiguration(), mockConfig);

            //Assert
            mockConfig.VerifyAllExpectations();
        }
    }
}