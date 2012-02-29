using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class IsSameHelpers
    {
         public static void IsByReferenceSame<T>(this T actual, T expected) where T : class
         {
             Assert.AreSame(expected,actual, "Actual and Expected are in different memory locations");
         }
    }
}