using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class ShouldBeHelpers
    {
        public static void ShouldBe<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected,actual, string.Format("Actual {0} was not equal to Expected {1}", actual, expected));
        }
    }
}
