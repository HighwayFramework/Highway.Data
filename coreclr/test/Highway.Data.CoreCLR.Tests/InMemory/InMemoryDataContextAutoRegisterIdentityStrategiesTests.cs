using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.CoreCLR.Tests.InMemory.Domain;
using NUnit.Framework;

namespace Highway.Data.CoreCLR.Tests.InMemory
{
    [TestFixture]
    public class InMemoryDataContextAutoRegisterIdentityStrategiesTests
    {
        class Entity : IIdentifiable<int>
        {
            public Entity()
            {
                MyProperties = new List<AnotherProperty>();
            }
            public int Id { get; set; }
            public AnotherProperty MyProperty { get; set; }
            public IList<AnotherProperty> MyProperties { get; set; }
        }

        class AnotherProperty : IIdentifiable<short>
        {
            public short Id { get; set; }
        }

        private InMemoryDataContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new InMemoryDataContext();
        }

        [Test]
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

        [Test]
        public void Add_ShouldNotChangeIdWhenIdExisting()
        {
            //Arrange
            var entity = new Entity { Id = 25 };

            //Act
            _context.Add(entity);

            //Assert
            entity.Id.Should().Be(25);
        }

        [Test]
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

        [Test]
        public void Add_ShouldUseIdentityForRelatedTypes()
        {
            //Arrange
            var entity = new Entity();
            entity.MyProperty = new AnotherProperty();

            //Act
            _context.Add(entity);
            _context.Commit();

            //Assert
            entity.MyProperty.Id.Should().NotBe(0);
        }


        [Test]
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
    }
}
