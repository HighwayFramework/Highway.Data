using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Highway.Data.Tests
{
    [TestClass]
    public abstract class BaseTest<T> where T : class
    {
        protected T target;

        public TestContext TestContext { get; set; }

        public virtual void BeforeAllTests() { }
        public virtual void BeforeEachTest() 
        {
            Console.WriteLine("--- " + TestContext.TestName);
        }
        public virtual void AfterEachTest() 
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                Console.WriteLine("=== Result : " + TestContext.CurrentTestOutcome.ToString());
            if ((target as IDisposable) != null)
                using (target as IDisposable) { }
        }
        public virtual void AfterAllTests() { }

        [ClassInitialize]
        private void SetupAllTests()
        {
            BeforeAllTests();
        }

        [TestInitialize]
        public void SetupEachTest()
        {
            BeforeEachTest();
        }

        [TestCleanup]
        public void TeardownEachTest()
        {
            AfterEachTest();
        }

        [ClassCleanup]
        public void TeardownAllTests()
        {
            AfterAllTests();
        }
    }
}
