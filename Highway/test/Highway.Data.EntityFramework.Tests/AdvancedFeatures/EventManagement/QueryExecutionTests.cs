using System.Collections.Generic;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;
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
            var context = new Repository(new DomainContext<TestDomain>(domain),domain);
            context.Commit();

            //assert
            interceptor.WasCalled.Should().BeTrue();

        }
    }
}