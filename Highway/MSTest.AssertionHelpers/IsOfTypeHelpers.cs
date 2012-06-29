using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class IsOfTypeHelpers
    {
         public static void IsOfType<T>(this T item, Type expectedType)
         {
         }

        public static void IsOfType<T>(this object item)
        {
            Assert.IsInstanceOfType(item, typeof(T), string.Format("Object of Type {0} was not the expected Type of {1}", item.GetType().Name, typeof(T).Name));

        }
    }
}