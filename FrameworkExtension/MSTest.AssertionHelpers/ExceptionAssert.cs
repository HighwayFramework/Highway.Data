using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public class ExceptionAssert
    {
         public static void Throws<T>(Action action)
         {
             try
             {
                 action();
             }
             catch (Exception e)
             {
                 Assert.IsInstanceOfType(e, typeof(T), string.Format("The thrown exception is of type {0} instead of the expected type {1}", e.GetType().Name, typeof(T).Name));
                 return;
             }
             Assert.Fail("Doesn't Throw any exception");
         }
    }
}