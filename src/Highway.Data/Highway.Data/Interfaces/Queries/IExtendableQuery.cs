using System;
using System.Linq.Expressions;

namespace Highway.Data
{
    /// <summary>
    ///     This interface is used to allow for extension of prebuilt queries
    /// </summary>
    public interface IExtendableQuery
    {
        /// <summary>
        ///     Adds a method to the expression in the query object
        /// </summary>
        /// <param name="methodName">The name of the method to be added i.e. GroupBy</param>
        /// <param name="generics">Any type parameters needed by the method to be added</param>
        /// <param name="parameters">Any object parameters needed by the method to be added</param>
        void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters);
    }
}