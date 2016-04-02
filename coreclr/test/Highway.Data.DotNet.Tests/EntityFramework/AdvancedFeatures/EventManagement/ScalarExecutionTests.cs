using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Factories;
using Highway.Data.Interceptors.Events;
using Highway.Data.Tests.TestDomain;
using NUnit.Framework;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    [TestFixture]
    public class ScalarExecutionTests
    {
        [Test]
        public void ShouldAddSingleInterceptorForPreScalarExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeScalar>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyScalar());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [Test]
        public void ShouldAddTwoInterceptorForPreScalarExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeScalar>(1);
            var interceptorTwo = new TestEventInterceptor<BeforeScalar>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyScalar());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }

        [Test]
        public void ShouldAddSingleInterceptorForAfterScalarExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterScalar>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyScalar());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [Test]
        public void ShouldAddTwoInterceptorForAfterScalarExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterScalar>(1);
            var interceptorTwo = new TestEventInterceptor<AfterScalar>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new DomainRepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Find(new EmptyScalar());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }
    }

    public class EmptyScalar : Scalar<Foo>
    {
        public EmptyScalar()
        {
            ContextQuery = c => (Foo) null;
        }
    }
}
