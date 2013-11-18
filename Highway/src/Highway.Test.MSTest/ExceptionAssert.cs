#region

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Highway.Test.MSTest
{
    public static class ExceptionAssert
    {
        /// <summary>
        ///     This method gives us a wrapper to catch an exception
        /// </summary>
        /// <typeparam name="T">The Type of Exception that should be thrown</typeparam>
        /// <param name="action">The execution that should throw the exception</param>
        public static void ShouldThrowException<T>(this Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof (T),
                    string.Format(
                        "The thrown exception is of type {0} instead of the expected type {1}",
                        e.GetType().Name, typeof (T).Name));
                return;
            }
            Assert.Fail("Doesn't Throw any exception");
        }
    }
}