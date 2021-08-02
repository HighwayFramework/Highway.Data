using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory
{
    [TestClass]
    public class InMemoryDataContextAutoRegisterIdentityStrategiesTests
    {
        private InMemoryDataContext _context;

        [TestMethod]
        public void Add_ShouldNotChangeIdWhenIdExisting()
        {
            //Arrange
            var entity = new Entity { Id = 25 };

            //Act
            _context.Add(entity);

            //Assert
            entity.Id.Should().Be(25);
        }

        [TestMethod]
        public void Add_ShouldUseIdentityForRelatedCollectionTypes()
        {
            //Arrange
            var entity = new Entity();
            entity.MyProperties.Add(new AnotherProperty());

            //Act
            _context.Add(entity);
            _context.Commit();

            //Assert
            entity.MyProperties.Single().Id.Should().NotBe(0);
        }

        [TestMethod]
        public void Add_ShouldUseIdentityForRelatedTypes()
        {
            //Arrange
            var entity = new Entity
            {
                MyProperty = new AnotherProperty()
            };

            //Act
            _context.Add(entity);
            _context.Commit();

            //Assert
            entity.MyProperty.Id.Should().NotBe(0);
        }

        [TestMethod]
        public void Add_ShouldUseIdentityForType()
        {
            //Arrange
            var entity = new Entity();

            //Act
            _context.Add(entity);
            _context.Commit();

            //Assert
            entity.Id.Should().NotBe(0);
        }

        [TestMethod]
        public void Commit_ShouldUseIdentityForRelatedCollectionTypes()
        {
            //Arrange
            var entity = new Entity();
            _context.Add(entity);
            _context.Commit();
            entity.MyProperties.Add(new AnotherProperty());

            //Act
            _context.Commit();

            //Assert
            entity.MyProperties.Single().Id.Should().NotBe(0);
        }

        [TestInitialize]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        private class Entity : IIdentifiable<int>
        {
            public Entity()
            {
                MyProperties = new List<AnotherProperty>();
            }

            public int Id { get; set; }

            public IList<AnotherProperty> MyProperties { get; }

            public AnotherProperty MyProperty { get; set; }
        }

        private class AnotherProperty : IIdentifiable<short>
        {
            public short Id { get; set; }
        }
    }
}
