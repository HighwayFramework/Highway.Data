using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests
{
    [TestClass]
    public abstract class BaseTest<T> where T : class
    {
        protected T target;

        public TestContext TestContext { get; set; }

        public virtual void BeforeAllTests()
        {
        }

        public virtual void BeforeEachTest()
        {
            Console.WriteLine("--- " + TestContext.TestName);
        }

        public virtual void AfterEachTest()
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                Console.WriteLine("=== Result : " + TestContext.CurrentTestOutcome);
            if ((target as IDisposable) != null)
                using (target as IDisposable)
                {
                }
        }

        public virtual void AfterAllTests()
        {
        }

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
