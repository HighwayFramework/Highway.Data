using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.ReadonlyTests
{
    [TestClass]
    public class SetUpFixture
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Console.WriteLine(context.TestName);
        }

        [TestMethod]
        public void TestOne()
        {
            1.Should().Be(1);
        }
    }
}
