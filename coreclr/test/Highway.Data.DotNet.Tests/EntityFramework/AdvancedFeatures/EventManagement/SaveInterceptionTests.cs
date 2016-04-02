using System.Collections.Generic;
using FluentAssertions;
using Highway.Data.EntityFramework.Tests.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;
using Highway.Data.Repositories;
using NUnit.Framework;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    [TestFixture]
    public class SaveInterceptionTests
    {
        [Test]
        public void ShouldRegisterOneEventHandlerForPreSave()
        {
            //arrange
            var domain = new TestDomain();
            var testPreSaveInterceptor = new TestEventInterceptor<BeforeSave>(1);
            domain.Events = new List<IInterceptor>
            {
                testPreSaveInterceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repo = new DomainRepository<TestDomain>(new DomainContext<TestDomain>(domain),domain);
            repo.Context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldRegisterTwoEventHandlersForPreSaveAndHonorPriority()
        {
            //arrange
            var domain = new TestDomain();
            var testPreSaveInterceptor = new TestEventInterceptor<BeforeSave>(1);
            var testPreSaveInterceptor2 = new TestEventInterceptor<BeforeSave>(2);
            domain.Events = new List<IInterceptor>
            {
                testPreSaveInterceptor,
                testPreSaveInterceptor2,
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repo = new DomainRepository<TestDomain>(new DomainContext<TestDomain>(domain), domain);
            repo.Context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.Should().BeTrue();
            testPreSaveInterceptor2.WasCalled.Should().BeTrue();
            testPreSaveInterceptor.CallTime.Should().BeBefore(testPreSaveInterceptor2.CallTime);
        }

        [Test]
        public void ShouldRegisterOneEventHandlerForPostSave()
        {
            //arrange
            var domain = new TestDomain();
            var testPreSaveInterceptor = new TestEventInterceptor<AfterSave>(1);
            domain.Events = new List<IInterceptor>
            {
                testPreSaveInterceptor
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repo = new DomainRepository<TestDomain>(new DomainContext<TestDomain>(domain), domain);
            repo.Context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldRegisterTwoEventHandlersForPostSaveAndHonorPriority()
        {
            //arrange
            var domain = new TestDomain();
            var testPreSaveInterceptor = new TestEventInterceptor<AfterSave>(1);
            var testPreSaveInterceptor2 = new TestEventInterceptor<AfterSave>(2);
            domain.Events = new List<IInterceptor>
            {
                testPreSaveInterceptor,
                testPreSaveInterceptor2,
            };
            domain.ConnectionString = Settings.Default.Connection;

            //act
            var repo = new DomainRepository<TestDomain>(new DomainContext<TestDomain>(domain), domain);
            repo.Context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.Should().BeTrue();
            testPreSaveInterceptor2.WasCalled.Should().BeTrue();
            testPreSaveInterceptor.CallTime.Should().BeBefore(testPreSaveInterceptor2.CallTime);
        }
    }
}
