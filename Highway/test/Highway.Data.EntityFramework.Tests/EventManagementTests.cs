#region

using System.Collections.Generic;
using Highway.Data.Domain;
using Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement;
using Highway.Data.EntityFramework.Tests.Properties;
using Highway.Data.EventManagement.Interfaces;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Data.EntityFramework.Tests.EventManagement
{
    [TestClass]
    public class EventManagementTests
    {
        [TestMethod]
        public void ShouldRegisterOneEventHandlerForOneEvent()
        {
            //arrange 
            var domain = new TestDomain();
            var testPreSaveInterceptor = new TestPreSaveInterceptor();
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
    }

    public class TestDomain : IDomain
    {
        public List<IInterceptor> Events { get; set; }
        public string ConnectionString { get; set; }
        public IMappingConfiguration Mappings { get; set; }
        public IContextConfiguration Context { get; set; }
    }
}