using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Test.MSTest
{
    public static class StringHelpers
    {
        public static void ShouldContain(this string input, string contents)
        {
           if (!input.ToLowerInvariant().Contains(contents))
           {
               Assert.Fail(string.Format("input ({0}) doesn't contain search string ({1})", input, contents));
           }
        }

        public static void ShouldContainWithCase(this string input, string contents)
        {
            Assert.IsTrue(input.Contains(contents));
        }
    }
}