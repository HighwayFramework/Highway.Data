#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Test.MSTest
{
    public static class ShouldBeSameHelpers
    {
        public static void ShouldBeSame<T>(this T actual, T expected) where T : class
        {
            Assert.AreSame(expected, actual, "Actual and Expected are in different memory locations");
        }
    }
}