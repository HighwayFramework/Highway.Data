using System;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Factory;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Tests.Mapping;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    [TestClass]
    public class Given_A_Generic_Repository_Factory
    {
        private IMappingConfiguration fooMapping;
        private IMappingConfiguration barMapping;
        private IMappingConfiguration bazMapping;
        private IMappingConfiguration quxMapping;

        [TestInitialize]
        public void Setup()
        {
            fooMapping = MockRepository.GenerateMock<IMappingConfiguration>();
            fooMapping.Expect(
                m =>
                m.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything)).Repeat.Once();
            
            barMapping = MockRepository.GenerateMock<IMappingConfiguration>();
            barMapping.Expect(
                m =>
                m.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything)).Repeat.Once();

            bazMapping = MockRepository.GenerateMock<IMappingConfiguration>();
            bazMapping.Expect(
                m =>
                m.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything)).Repeat.Once();

            quxMapping = MockRepository.GenerateMock<IMappingConfiguration>();
            quxMapping.Expect(
                m =>
                m.ConfigureModelBuilder(Arg<DbModelBuilder>.Is.Anything)).Repeat.Once();


        }

        [TestMethod]
        public void Should_Get_Mappings_Specific_To_The_Type_Requested()
        {
            //Arrange
            Func<Type, IMappingConfiguration> mappingsDelegate = x =>
                                                                     {
                                                                         if (x == typeof (Foo)) return fooMapping;
                                                                         return null;
                                                                     };
            var target = new RepositoryFactory(Settings.Default.Connection, mappingsDelegate);

            //Act
            var repository = target.Create<Foo>();
            try
            {
                repository.Context.AsQueryable<Foo>().ToList();
            }
            catch (Exception)
            {
                //Suppress the error from the context. This allows us to test the mappings peice without having to actually map.
            }
            //Assert
            fooMapping.VerifyAllExpectations();
        }

        [TestMethod]
        public void Should_Get_Mappings_Specific_To_The_Type_Requested_When_Multiple_Types_Are_Requested()
        {
            //Arrange
            Func<Type, IMappingConfiguration> mappingsDelegate = x =>
            {
                if (x == typeof(Foo)) return fooMapping;
                if (x == typeof(Bar)) return barMapping;
                if (x == typeof(Baz)) return bazMapping;
                if (x == typeof(Qux)) return quxMapping;
                return null;
            };
            var target = new RepositoryFactory(Settings.Default.Connection, mappingsDelegate);

            //Act
            var repository = target.Create<Foo,Bar,Baz,Qux>();
            try
            {
                repository.Context.AsQueryable<Foo>().ToList();
            }
            catch (Exception)
            {
                //Suppress the error from the context. This allows us to test the mappings peice without having to actually map.
            }

            //Assert
            fooMapping.VerifyAllExpectations();
            barMapping.VerifyAllExpectations();
            bazMapping.VerifyAllExpectations();
            quxMapping.VerifyAllExpectations();
        }


    }
}