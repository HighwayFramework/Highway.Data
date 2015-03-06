using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Highway.Data.Security.DataEntitlements;
using Highway.Data.Security.DataEntitlements.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.Security
{
    [TestClass]
    public class ParameterToMemberRebinderTests
    {
        [TestMethod]
        public void ShouldChangeParameterForLambdaIntoMemberAccessor()
        {
            var data = new List<ExampleLeaf>
            {
                new ExampleLeaf
                {
                    SecuredRoot = new ExampleRoot
                    {
                        Id = 1
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoot = new ExampleRoot
                    {
                        Id = 4
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoot = new ExampleRoot
                    {
                        Id = 5
                    }
                }
            };

            Expression<Func<ExampleLeaf, ExampleRoot>> selector = x => x.SecuredRoot;
            List<long> ids = new List<long> { 1, 2, 3 };
            Expression<Func<ExampleRoot, bool>> predicate = x => ids.Contains(x.Id);

            // x => ids.Contains(x.SecuredRoot.Id)

            var resultingCombined = ParameterToMemberExpressionRebinder.CombineSinglePropertySelectorWithPredicate(selector, predicate);

            var resultingPredicate = resultingCombined.Compile();

            var results = data.Where(resultingPredicate);

            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldChangeCollectionParameterForLambdaIntoMemberAccessor()
        {
            var data = new List<ExampleLeaf>
            {
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 1
                        }
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 4
                        }
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 5
                        }
                    }
                }
            };

            Expression<Func<ExampleLeaf, IEnumerable<ExampleRoot>>> selector = x => x.SecuredRoots;
            List<long> ids = new List<long> { 1, 2, 3 };
            Expression<Func<IEnumerable<ExampleRoot>, bool>> predicate = x => x.Any(c => ids.Contains(c.Id));

            // x => x.SecuredRoots.Any(c => ids.Contains(c.Id))

            var resultingCombined = ParameterToMemberExpressionRebinder.CombineSinglePropertySelectorWithPredicate(selector, predicate);

            var resultingPredicate = resultingCombined.Compile();

            var results = data.Where(resultingPredicate);

            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldReflectivelyChangeParameterForLambdaIntoMemberAccessor()
        {
            var data = new List<ExampleLeaf>
            {
                new ExampleLeaf
                {
                    SecuredRoot = new ExampleRoot
                    {
                        Id = 1
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoot = new ExampleRoot
                    {
                        Id = 4
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoot = new ExampleRoot
                    {
                        Id = 5
                    }
                }
            };

            Expression<Func<ExampleLeaf, ExampleRoot>> selector = x => x.SecuredRoot;
            LambdaExpression selectorExpression = selector;
            List<long> ids = new List<long> { 1, 2, 3 };
            Expression<Func<ExampleRoot, bool>> predicate = x => ids.Contains(x.Id);
            LambdaExpression predicateExpression = predicate;

            // x => ids.Contains(x.SecuredRoot.Id)

            var resultingCombined = ParameterToMemberExpressionRebinder.CombineSinglePropertySelectorWithPredicate(selectorExpression, predicateExpression);

            var resultingPredicate = (Func<ExampleLeaf, bool>)resultingCombined.Compile();

            var results = data.Where(resultingPredicate);

            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldReflectivelyChangeCollectionParameterForLambdaIntoMemberAccessor()
        {
            var data = new List<ExampleLeaf>
            {
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 1
                        }
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 4
                        }
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 5
                        }
                    }
                }
            };

            Expression<Func<ExampleLeaf, IEnumerable<ExampleRoot>>> selector = x => x.SecuredRoots;
            LambdaExpression selectorExpression = selector;
            List<long> ids = new List<long> { 1, 2, 3 };
            Expression<Func<IEnumerable<ExampleRoot>, bool>> predicate = x => x.Any(c => ids.Contains(c.Id));
            LambdaExpression predicateExpression = predicate;

            // x => x.SecuredRoots.Any(c => ids.Contains(c.Id))

            var resultingCombined = ParameterToMemberExpressionRebinder.CombineSinglePropertySelectorWithPredicate(selectorExpression, predicateExpression);

            var resultingPredicate = (Func<ExampleLeaf, bool>)resultingCombined.Compile();

            var results = data.Where(resultingPredicate);

            results.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldReflectivelyUpdatePredicateForCollectionAccessors()
        {
            var data = new List<ExampleLeaf>
            {
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 1
                        }
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 4
                        }
                    }
                },
                new ExampleLeaf
                {
                    SecuredRoots = new[]
                    {
                        new ExampleRoot
                        {
                            Id = 5
                        }
                    }
                }
            };

            Expression<Func<ExampleLeaf, IEnumerable<ExampleRoot>>> selector = x => x.SecuredRoots;
            LambdaExpression selectorExpression = selector;
            List<long> ids = new List<long> { 1, 2, 3 };
            Expression<Func<ExampleRoot, bool>> predicate = x => ids.Contains(x.Id);
            LambdaExpression predicateExpression = predicate;

            // x => x.SecuredRoots.Any(c => ids.Contains(c.Id))

            var resultingCombined = SecuredExtensions.CombineCollectionPropertySelectorWithPredicate(selectorExpression, predicateExpression);

            var resultingPredicate = (Func<ExampleLeaf, bool>)resultingCombined.Compile();

            var results = data.Where(resultingPredicate);

            results.Should().HaveCount(1);
        }
    }
}
