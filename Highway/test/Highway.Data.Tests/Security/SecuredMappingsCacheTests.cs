using System;
using System.Linq;
using FluentAssertions;
using Highway.Data.Security.DataEntitlements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.Security
{
    [TestClass]
    public class SecuredMappingsCacheTests
    {
        [TestMethod]
        public void CanCreateSecuredMappingsCache()
        {
            // Act
            var exampleMappingConfiguration = new ISecuredRelationshipBuilder[] { _exampleSecuredRelationshipBuilder };
            Action action = () => new SecuredMappingsCache(exampleMappingConfiguration);

            // Assert
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void CanResolveSecuredMappingsForExampleSecuredMappings()
        {
            // Arrange
            var exampleMappingConfigurations = new ISecuredRelationshipBuilder[] { _exampleSecuredRelationshipBuilder };
            var securedMappingsCache = new SecuredMappingsCache(exampleMappingConfigurations);

            // Act
            var securedMappings = securedMappingsCache.GetSecuredRelationships<ExampleLeaf>().ToList();

            // Assert
            securedMappings.Should().NotBeNull();
            securedMappings.Should().HaveCount(2);
        }

        [TestMethod]
        public void DuplicateSecurableRelationshipsAreNotReturned()
        {
            // Arrange
            var exampleMappingConfigurations = new ISecuredRelationshipBuilder[] { _exampleSecuredRelationshipBuilder, _exampleSecuredRelationshipBuilder2 };
            var securedMappingsCache = new SecuredMappingsCache(exampleMappingConfigurations);

            // Act
            var securedMappings = securedMappingsCache.GetSecuredRelationships<ExampleLeaf>().ToList();

            // Assert
            securedMappings.Should().NotBeNull();
            securedMappings.Should().HaveCount(2);
        }

        [TestInitialize]
        public void TestFixtureSetUp()
        {
            _exampleSecuredRelationshipBuilder = new NullPermissiveExampleSecuredRelationshipBuilder();
            _exampleSecuredRelationshipBuilder2 = new NullPermissiveExampleSecuredRelationshipBuilder();
        }

        private NullPermissiveExampleSecuredRelationshipBuilder _exampleSecuredRelationshipBuilder;
        private NullPermissiveExampleSecuredRelationshipBuilder _exampleSecuredRelationshipBuilder2;
    }
}
