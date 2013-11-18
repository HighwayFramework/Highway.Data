#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Test.MSTest
{
    public static class StringHelpers
    {
        public static void ShouldContain(this string input, string contents)
        {
            if (!input.ToLowerInvariant().Contains(contents))
            {
                Assert.Fail("input ({0}) doesn't contain search string ({1})", input, contents);
            }
        }

        public static void ShouldContainWithCase(this string input, string contents)
        {
            Assert.IsTrue(input.Contains(contents));
        }
    }
}