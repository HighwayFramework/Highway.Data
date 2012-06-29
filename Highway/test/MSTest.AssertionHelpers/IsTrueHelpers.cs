using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class IsTrueHelpers
    {
        public static void IsTrue<T>(this T actual, Func<T,bool> condition)
        {
            Assert.IsTrue(condition(actual), "Value was not true");
        }

        public static void IsTrue(this bool actual)
        {
            Assert.IsTrue(actual, "Value was not true");
        }
    }
}