using System.Collections.Generic;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Factories;
using Highway.Data.Interceptors.Events;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    [TestClass]
    public class CommandExecutionTests
    {
        [TestMethod]
        public void ShouldAddSingleInterceptorForPreCommandExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeCommand>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Execute(new EmptyCommand());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [TestMethod]
        public void ShouldAddTwoInterceptorForPreCommandExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<BeforeCommand>(1);
            var interceptorTwo = new TestEventInterceptor<BeforeCommand>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Execute(new EmptyCommand());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }

        [TestMethod]
        public void ShouldAddSingleInterceptorForAfterCommandExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterCommand>(1);
            domain.Events = new List<IInterceptor>
            {
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Execute(new EmptyCommand());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }

        [TestMethod]
        public void ShouldAddTwoInterceptorForAfterCommandExecution()
        {
            //Arrange
            var domain = new TestDomain();
            var interceptor = new TestEventInterceptor<AfterCommand>(1);
            var interceptorTwo = new TestEventInterceptor<AfterCommand>(2);
            domain.Events = new List<IInterceptor>
            {
                interceptorTwo,
                interceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repository = new RepositoryFactory(new[] { domain }).Create<TestDomain>();
            repository.Execute(new EmptyCommand());

            //assert
            interceptor.WasCalled.Should().BeTrue();
            interceptorTwo.WasCalled.Should().BeTrue();
            interceptor.CallTime.Should().BeBefore(interceptorTwo.CallTime);

        }
    }

    public class EmptyCommand : Command
    {
        public EmptyCommand()
        {
            ContextQuery = c => { };
        }
    }
}