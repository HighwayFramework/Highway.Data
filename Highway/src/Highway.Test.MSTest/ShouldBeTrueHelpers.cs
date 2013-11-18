#region

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Test.MSTest
{
    public static class ShouldBeTrueHelpers
    {
        public static void ShouldBeTrue<T>(this T actual, Func<T, bool> condition)
        {
            Assert.IsTrue(condition(actual), "Value was not true");
        }

        public static void ShouldBeTrue(this bool actual)
        {
            Assert.IsTrue(actual, "Value was not true");
        }
    }
}