using System.Linq.Expressions;
using System.Data.Entity;
using Highway.Data.Interfaces;

// ReSharper disable CheckNamespace
namespace Highway.Data.QueryObjects
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension to allow for mulitple table includes
    /// </summary>
    public static class IncludeExtension
    {
        /// <summary>
        /// Takes the specified number of records
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuery<T> Include<T>(this IQuery<T> extend, int count)
        {
            var generics = new[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Include", generics, parameters);
            return extend;
        }

    }
}