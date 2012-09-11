using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Test.MSTest
{
    public static class StringHelpers
    {
        public static void ShouldContain(this string input, string contents)
        {
            Assert.IsTrue(input.ToLowerInvariant().Contains(contents));
        }

        public static void ShouldContainWithCase(this string input, string contents)
        {
            Assert.IsTrue(input.Contains(contents));
        }
    }
}