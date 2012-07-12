using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Test.MSTest
{
    public static class ShouldBeNotEqualHelpers
    {
        public static void ShouldBeNotEqual<T>(this T actual, T expected)
         {
             Assert.AreEqual(expected,actual,string.Format("Actual {1} differs from the Expected {1}", actual, expected));
         }
    }
}