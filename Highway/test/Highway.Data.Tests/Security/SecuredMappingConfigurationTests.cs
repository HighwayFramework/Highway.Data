using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.Security
{
    [TestClass]
    public class SecuredMappingConfigurationTests
    {
        [TestMethod]
        public void ShouldAddCollectionToGlobalCacheWhenCalled()
        {
            var mappings = new NullPermissiveExampleSecuredRelationshipBuilder();

            var maps = mappings.BuildRelationships();

            maps.Where(x => x.Secured == typeof(ExampleLeaf) && x.SecuredBy == typeof(IEnumerable<ExampleRoot>)).Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldAddSinglePropertyToGlobalCacheWhenCalled()
        {
            var mappings = new NullPermissiveExampleSecuredRelationshipBuilder();

            var maps = mappings.BuildRelationships();

            maps.Where(x => typeof(ExampleLeaf).IsAssignableFrom(x.Secured) && typeof(ExampleRoot).IsAssignableFrom(x.SecuredBy)).Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldFilterBasedOnSecurity()
        {
            var mappings = new NullPermissiveExampleSecuredRelationshipBuilder();

            var maps = mappings.BuildRelationships();
            var map = maps.Single(x => x.Secured == typeof(ExampleLeaf) && x.SecuredBy == typeof(ExampleRoot));
            var listToFilter = new List<ExampleLeaf>
                                   {
                                       new ExampleLeaf
                                           {
                                               SecuredRoot = new ExampleRoot { Id = 1 }
                                           },
                                       new ExampleLeaf
                                           {
                                               SecuredRoot = new ExampleRoot { Id = 2 }
                                           },
                                       new ExampleLeaf
                                           {
                                               SecuredRoot = new ExampleRoot { Id = 3 }
                                           }
                                   };

            var permissions = new HashSet<long> { 1, 2 };

            var results = map.ApplySecurity(listToFilter.AsQueryable(), permissions).ToList();

            results.Should().HaveCount(2);
        }

        [TestMethod]
        public void ShouldFilterCollectionBasedOnSecurity()
        {
            var mappings = new NullPermissiveExampleSecuredRelationshipBuilder();

            var maps = mappings.BuildRelationships();
            var map = maps.Single(x => x.Secured == typeof(ExampleLeaf) && x.SecuredBy == typeof(IEnumerable<ExampleRoot>));
            var listToFilter = new List<ExampleLeaf>
                                   {
                                       new ExampleLeaf
                                           {
                                               SecuredRoots = new List<ExampleRoot>
                                                                  {
                                                                      new ExampleRoot { Id = 1 }, 
                                                                  }
                                           }, 
                                       new ExampleLeaf
                                           {
                                               SecuredRoots = new List<ExampleRoot>
                                                                  {
                                                                      new ExampleRoot { Id = 2 }, 
                                                                  }
                                           }
                                   };

            var permissions = new HashSet<long> { 1 };

            var results = map.ApplySecurity(listToFilter.AsQueryable(), permissions).ToList();

            results.Should().HaveCount(1);
        }
    }
}
