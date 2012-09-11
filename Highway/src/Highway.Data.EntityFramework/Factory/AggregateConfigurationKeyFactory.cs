using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data
{
    public abstract class AggregateConfigurationKeyFactory
    {
        /// <summary>
        /// Generates the key for lookup on registration of types for Aggregate root bounded contexts
        /// </summary>
        /// <typeparam name="T1">The aggregate root for the context</typeparam>
        /// <returns>The key for named registration</returns>
        public static string GenerateKey<T1>() where T1 : class 
        {
            return GenerateKey(typeof(T1));
        }

        /// <summary>
        /// Generates the key for lookup on registration of types for Aggregate root bounded contexts
        /// </summary>
        /// <typeparam name="T1">One of the aggregate roots for the context</typeparam>
        /// <typeparam name="T2">One of the aggregate roots for the context </typeparam>
        /// <returns>The key for named registration</returns>
        public static string GenerateKey<T1,T2>()
            where T1 : class 
            where T2 : class
        {
            return GenerateKey(typeof(T1), typeof(T2));
        }

        /// <summary>
        /// Generates the key for lookup on registration of types for Aggregate root bounded contexts
        /// </summary>
        /// <typeparam name="T1">One of the aggregate roots for the context</typeparam>
        /// <typeparam name="T2">One of the aggregate roots for the context</typeparam>
        /// <typeparam name="T3">One of the aggregate roots for the context</typeparam>
        /// <returns>The key for named registration</returns>
        public static string GenerateKey<T1, T2, T3>()
            where T1 : class
            where T2 : class
            where T3 : class
        {
            return GenerateKey(typeof(T1),typeof(T2),typeof(T3));
        }


        /// <summary>
        /// Generates the key for lookup on registration of types for Aggregate root bounded contexts
        /// </summary>
        /// <typeparam name="T1">One of the aggregate roots for the context</typeparam>
        /// <typeparam name="T2">One of the aggregate roots for the context</typeparam>
        /// <typeparam name="T3">One of the aggregate roots for the context</typeparam>
        /// <typeparam name="T4">One of the aggregate roots for the context</typeparam>
        /// <returns>The key for named registration</returns>
        public static string GenerateKey<T1, T2, T3, T4>()
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            return GenerateKey(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        internal static string GenerateKey(params Type[] types)
        {
            return string.Join(",", types.Select(x => x.FullName));
        }

    }
}