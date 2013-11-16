using System.Collections.Generic;
using Highway.Data.Domain;
using Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement;
using Highway.Data.EventManagement.Interfaces;
using Highway.Test.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

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
            domain.Events = new List<IInterceptor>()
            {
                testPreSaveInterceptor
            };

            //act
            var context = new DomainContext<TestDomain>(domain);
            context.Commit();

            //assert
            testPreSaveInterceptor.WasCalled.ShouldBeTrue();
        }
    }

    public class TestDomain : IDomain
    {
        public string ConnectionString { get; set; }
        public IMappingConfiguration Mappings { get; set; }
        public IContextConfiguration Context { get; set; }
        public List<IInterceptor> Events { get; set; }
    }
}