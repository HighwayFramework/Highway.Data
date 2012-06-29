using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.AssertionHelpers
{
    public static class IsNotNullHelpers
    {
         public static void IsNotNull(this object item)
         {
             Assert.IsNotNull(item, "The expected object is null");
         }
    }
}