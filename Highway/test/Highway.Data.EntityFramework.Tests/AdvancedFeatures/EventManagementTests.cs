#region

using System;
using System.Collections.Generic;
using FluentAssertions;
using Highway.Data.Domain;
using Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.EntityFramework.Tests.EventManagement
{
    [TestClass]
    public class EventManagementTests
    {
        [TestMethod]
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
            var context = new DomainContext<TestDomain>(domain);
            context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.ShouldBeTrue();
        }

        [TestMethod]
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
            var context = new DomainContext<TestDomain>(domain);
            context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.ShouldBeTrue();
            testPreSaveInterceptor2.WasCalled.ShouldBeTrue();
            testPreSaveInterceptor.CallTime.Should().BeBefore(testPreSaveInterceptor2.CallTime);
        }

        [TestMethod]
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
            var context = new DomainContext<TestDomain>(domain);
            context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.ShouldBeTrue();
        }

        [TestMethod]
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
            var context = new DomainContext<TestDomain>(domain);
            context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.ShouldBeTrue();
            testPreSaveInterceptor2.WasCalled.ShouldBeTrue();
            testPreSaveInterceptor.CallTime.Should().BeBefore(testPreSaveInterceptor2.CallTime);
        }
    }
}