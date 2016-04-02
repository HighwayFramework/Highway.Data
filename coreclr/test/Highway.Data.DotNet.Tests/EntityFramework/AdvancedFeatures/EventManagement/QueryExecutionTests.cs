using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Highway.Data.InMemory;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Factories;
using Highway.Data.Interceptors.Events;
using Highway.Data.PrebuiltInterceptors;
using Highway.Data.Repositories;
using Highway.Data.Tests.TestDomain;
using NUnit.Framework;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    [TestFixture]
    public class QueryExecutionTests
    {
        [Test]
        public void ShouldAddSingleInterceptorForPreQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeQuery>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new []{domain}).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [Test]
        public void ShouldAddTwoInterceptorForPreQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeQuery>(1);
            var interceptorTwo = new TestEventInterceptor<BeforeQuery>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new []{domain}).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }

        [Test]
        public void ShouldAddSingleInterceptorForAfterQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterQuery>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [Test]
        public void ShouldAddTwoInterceptorForAfterQueryExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterQuery>(1);
            var interceptorTwo = new TestEventInterceptor<AfterQuery>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }

        [Test]
        public void ShouldModifyTheQueryInExpressionToHaveAnAdditionalWhere()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new AppendWhere<Foo>(1, foo => foo.Name == "Test", typeof(AllFoos));
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var inMemoryDomainContext = new InMemoryDomainContext<TestDomain>();
            inMemoryDomainContext.Add(new Foo() {Name = "Test"});
            inMemoryDomainContext.Add(new Foo() {Name = "Should Not Show up"});
            inMemoryDomainContext.Commit();
            var repository = new DomainRepository<TestDomain>(inMemoryDomainContext, domain);
            var emptyQuery = new AllFoos();
            var results = repository.Find(emptyQuery);

            //assert
            results.Count().Should().Be(1);
        }
    }

    public class AllFoos : Query<Foo>
    {
        public AllFoos()
        {
            ContextQuery = c => c.AsQueryable<Foo>();
        }
    }
}
