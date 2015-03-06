using System.Linq;
using FluentAssertions;
using Highway.Data.EntityFramework.Security;
using Highway.Data.Security.DataEntitlements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Highway.Data.Tests.Security
{
    [TestClass]
    public class SecuredInMemoryTests
    {
        [TestMethod]
        public void ShouldReturnCorrectIntersationOfMatchingAndNonMatchingRecordsForNullPermissiveMappings()
        {
            // Arrange
            IEntitlementProvider sessionGovernor = MockRepository.GenerateMock<IEntitlementProvider>();
            sessionGovernor.Expect(x => x.IsEntitledToAll(typeof(ExampleRoot))).Return(false);
            sessionGovernor.Expect(x => x.GetEntitledIds(typeof(ExampleRoot))).Return(new long[] { 1, 2, 3 });

            var securedMappingsCache = new SecuredMappingsCache(new ISecuredRelationshipBuilder[] { new NullPermissiveExampleSecuredRelationshipBuilder() });
            var context = new SecuredInMemoryDomainContext<ExampleDomain>(sessionGovernor, securedMappingsCache);
            var singleNonMatchingLeaf = new ExampleLeaf
                                            {
                                                SecuredRoot = new ExampleRoot { Id = 4 }, 
                                                SecuredRoots = new[]
                                                                   {
                                                                       new ExampleRoot { Id = 6 }, 
                                                                       new ExampleRoot { Id = 7 }
                                                                   }
                                            };
            var pluralNonMatchingLeaf = new ExampleLeaf
                                            {
                                                SecuredRoot = new ExampleRoot { Id = 4 }, 
                                                SecuredRoots = new[]
                                                                   {
                                                                       new ExampleRoot { Id = 5 }
                                                                   }
                                            };
            var combinedNonMatchingLeaf = new ExampleLeaf
                                              {
                                                  SecuredRoot = new ExampleRoot
                                                                    {
                                                                        Id = 5
                                                                    }, 
                                                  SecuredRoots = new[]
                                                                     {
                                                                         new ExampleRoot { Id = 6 }, 
                                                                         new ExampleRoot { Id = 7 }
                                                                     }
                                              };
            var combinedMatchingLeaf = new ExampleLeaf
                                           {
                                               SecuredRoot = new ExampleRoot
                                                                 {
                                                                     Id = 1
                                                                 }, 
                                               SecuredRoots = new[]
                                                                  {
                                                                      new ExampleRoot { Id = 2 }, 
                                                                      new ExampleRoot { Id = 3 }
                                                                  }
                                           };

            context.Add(singleNonMatchingLeaf);
            context.Add(pluralNonMatchingLeaf);
            context.Add(combinedNonMatchingLeaf);
            context.Add(combinedMatchingLeaf);
            context.Commit();

            // Act
            var exampleLeaves = context.AsQueryable<ExampleLeaf>();
            var results = exampleLeaves.ToList();

            //Assert
            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldReturnCorrectIntersationOfMatchingAndNonMatchingRecordsForNullPropertiesOnNullPermissiveMappings()
        {
            // Arrange
            IEntitlementProvider sessionGovernor = MockRepository.GenerateMock<IEntitlementProvider>();
            sessionGovernor.Expect(x => x.IsEntitledToAll(typeof(ExampleRoot))).Return(false);
            sessionGovernor.Expect(x => x.GetEntitledIds(typeof(ExampleRoot))).Return(new long[] { 1, 2, 3 });

            var securedMappingsCache = new SecuredMappingsCache(new ISecuredRelationshipBuilder[] { new NullPermissiveExampleSecuredRelationshipBuilder() });
            var context = new SecuredInMemoryDomainContext<ExampleDomain>(sessionGovernor, securedMappingsCache);
            var singleNonMatchingLeaf = new ExampleLeaf
                                            {
                                                SecuredRoot = new ExampleRoot { Id = 4 }, 
                                                SecuredRoots = new[]
                                                                   {
                                                                       new ExampleRoot { Id = 5 }
                                                                   }
                                            };
            var pluralNonMatchingLeaf = new ExampleLeaf
                                            {
                                                SecuredRoot = new ExampleRoot { Id = 4 }, 
                                                SecuredRoots = new[]
                                                                   {
                                                                       new ExampleRoot { Id = 5 }
                                                                   }
                                            };
            var combinedNonMatchingLeaf = new ExampleLeaf
                                              {
                                                  SecuredRoot = new ExampleRoot
                                                                    {
                                                                        Id = 5
                                                                    }, 
                                                  SecuredRoots = new[]
                                                                     {
                                                                         new ExampleRoot { Id = 6 }, 
                                                                         new ExampleRoot { Id = 7 }
                                                                     }
                                              };
            var combinedMatchingLeaf = new ExampleLeaf
                                           {
                                               SecuredRoot = new ExampleRoot
                                                                 {
                                                                     Id = 1
                                                                 }, 
                                               SecuredRoots = new[]
                                                                  {
                                                                      new ExampleRoot { Id = 2 }, 
                                                                      new ExampleRoot { Id = 3 }
                                                                  }
                                           };
            var combinedNullMatchingLeaf = new ExampleLeaf();
            var singleNullMatchingLeaf = new ExampleLeaf
                                             {
                                                 SecuredRoots = new[]
                                                                    {
                                                                        new ExampleRoot { Id = 1 }
                                                                    }
                                             };
            var pluralNullMatchingLeaf = new ExampleLeaf
                                             {
                                                 SecuredRoot = new ExampleRoot { Id = 1 }, 
                                             };

            context.Add(singleNonMatchingLeaf);
            context.Add(pluralNonMatchingLeaf);
            context.Add(combinedNonMatchingLeaf);
            context.Add(combinedMatchingLeaf);
            context.Add(combinedNullMatchingLeaf);
            context.Add(singleNullMatchingLeaf);
            context.Add(pluralNullMatchingLeaf);
            context.Commit();

            // Act
            var exampleLeaves = context.AsQueryable<ExampleLeaf>();
            var results = exampleLeaves.ToList();

            //Assert
            results.Should().HaveCount(4);
        }

        [TestMethod]
        public void ShouldReturnLimitedResults()
        {
            IEntitlementProvider sessionGovernor = MockRepository.GenerateMock<IEntitlementProvider>();
            sessionGovernor.Expect(x => x.IsEntitledToAll(typeof(ExampleRoot))).Return(false);
            sessionGovernor.Expect(x => x.GetEntitledIds(typeof(ExampleRoot))).Return(new long[] { 1, 2, 3 });

            var securedMappingsCache = new SecuredMappingsCache(new ISecuredRelationshipBuilder[] { new NullPermissiveExampleSecuredRelationshipBuilder() });
            var context = new SecuredInMemoryDomainContext<ExampleDomain>(sessionGovernor, securedMappingsCache);
            var nonMatchingLeaf = new ExampleLeaf
                                      {
                                          SecuredRoot = new ExampleRoot { Id = 4 }
                                      };
            var matchingLeaf = new ExampleLeaf
                                   {
                                       SecuredRoot = new ExampleRoot { Id = 3 }
                                   };

            context.Add(nonMatchingLeaf);
            context.Add(matchingLeaf);
            context.Commit();

            var exampleLeaves = context.AsQueryable<ExampleLeaf>();
            var results = exampleLeaves.ToList();

            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldReturnLimitedResultsFromCollection()
        {
            // Arrange
            IEntitlementProvider sessionGovernor = MockRepository.GenerateMock<IEntitlementProvider>();
            sessionGovernor.Expect(x => x.IsEntitledToAll(typeof(ExampleRoot))).Return(false);
            sessionGovernor.Expect(x => x.GetEntitledIds(typeof(ExampleRoot))).Return(new long[] { 1, 2, 3 });

            var securedMappingsCache = new SecuredMappingsCache(new ISecuredRelationshipBuilder[] { new NullPermissiveExampleSecuredRelationshipBuilder() });
            var context = new SecuredInMemoryDomainContext<ExampleDomain>(sessionGovernor, securedMappingsCache);
            var nonMatchingLeaf = new ExampleLeaf
                                      {
                                          SecuredRoots = new[] { new ExampleRoot { Id = 4 } }
                                      };
            var matchingLeaf = new ExampleLeaf
                                   {
                                       SecuredRoots = new[] { new ExampleRoot { Id = 3 } }
                                   };

            context.Add(nonMatchingLeaf);
            context.Add(matchingLeaf);
            context.Commit();

            // Act
            var exampleLeaves = context.AsQueryable<ExampleLeaf>();
            var results = exampleLeaves.ToList();

            //Assert
            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void IsEntitledToAllWorks()
        {
            // Arrange
            
            // Act

            // Assert
            Assert.Inconclusive("We haven't written the test yet.");
        }
    }
}
