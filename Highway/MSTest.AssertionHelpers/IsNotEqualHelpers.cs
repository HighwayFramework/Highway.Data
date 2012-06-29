using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class IsNotEqualHelpers
    {
         public static void IsNotEqual<T>(this T actual, T expected)
         {
             Assert.AreEqual(expected,actual,string.Format("Actual {1} differs from the Expected {1}", actual, expected));
         }
    }
}