using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class IsEqualHelper
    {
         public static void IsEqual<T>(this T actual, T expected)
         {
             Assert.AreEqual(expected,actual,string.Format("Actual Value {0} differed from expected value {1}", actual, expected));
         }
    }
}