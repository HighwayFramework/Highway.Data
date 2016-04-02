using System;
using NUnit.Framework;

namespace Highway.Data.Tests
{
    [TestFixture]
    public abstract class BaseTest<T> where T : class
    {
        protected T target;

        public virtual void BeforeAllTests()
        {
        }

        public virtual void BeforeEachTest()
        {
        }

        public virtual void AfterEachTest()
        {
            if ((target as IDisposable) != null)
                using (target as IDisposable)
                {
                }
        }

        public virtual void AfterAllTests()
        {
        }

        [OneTimeSetUp]
        private void SetupAllTests()
        {
            BeforeAllTests();
        }

        [SetUp]
        public void SetupEachTest()
        {
            BeforeEachTest();
        }

        [TearDown]
        public void TeardownEachTest()
        {
            AfterEachTest();
        }

        [OneTimeTearDown]
        public void TeardownAllTests()
        {
            AfterAllTests();
        }
    }
}
