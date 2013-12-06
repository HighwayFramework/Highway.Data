using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EntityFramework.Tests.TestQueries;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Factories;
using Highway.Data.Interceptors.Events;
using Highway.Data.Tests.TestDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.EntityFramework.Tests.EventManagement
{
    [TestClass]
    public class QueryExecutionTests
    {
        [TestMethod]
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
            var repository = new RepositoryFactory(new []{domain}).Create<TestDomain>();
            repository.Find(new EmptyQuery());

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }
    }

    public class EmptyQuery : Query<Foo>
    {
        public EmptyQuery()
        {
            ContextQuery = c=> new List<Foo>().AsQueryable();
        }
    }
}