using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Highway.Data.Interfaces;
using Highway.Data;
using Microsoft.Practices.ServiceLocation;
using Highway.Data.EntityFramework.Builder;

namespace Highway.Data
{
    /// <summary>
    /// Factory for creating aggregate root based bounded contexts
    /// </summary>
    public abstract class AggregateContextFactory
    {
        private static IDataContext CreatContextFromConfiguration(IAggregateConfiguration configuration)
        {
            Type newContextType = DynamicAggregateContextBuilder.Create(configuration);
            var context = (IDataContext) Activator.CreateInstance(newContextType, new object[] { configuration });
            return context;
        }
        /// <summary>
        /// Creates an AggregateContext that is bound by the Type configured for it
        /// </summary>
        /// <typeparam name="T1">The Type to configure the context for</typeparam>
        /// <returns><seealso cref="IDataContext"/>The IDataContext for usage</returns>
        public static IDataContext Create<T1>()
            where T1 : class
        {
            var configuration = AggregateConfigurationFactory.GetConfigurationFor<T1>();
            return CreatContextFromConfiguration(configuration);
        }

        /// <summary>
        /// Creates an AggregateContext that is bound by the Type configured for it
        /// </summary>
        /// <typeparam name="T1">The Type to configure the context for</typeparam>
        /// <typeparam name="T2">The Type to configure the context for</typeparam>
        /// <returns><seealso cref="IDataContext"/>The IDataContext for usage</returns>
        public static IDataContext Create<T1, T2>() 
            where T1 : class 
            where T2 : class
        {
            var configuration = AggregateConfigurationFactory.GetConfigurationFor<T1,T2>();
            return CreatContextFromConfiguration(configuration);
        }


        /// <summary>
        /// Creates an AggregateContext that is bound by the Type configured for it
        /// </summary>
        /// <typeparam name="T1">The Type to configure the context for</typeparam>
        /// <typeparam name="T2">The Type to configure the context for</typeparam>
        /// <typeparam name="T3">The Type to configure the context for</typeparam>
        /// <returns><seealso cref="IDataContext"/>The IDataContext for usage</returns>
        public static IDataContext Create<T1, T2, T3>()
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var configuration = AggregateConfigurationFactory.GetConfigurationFor<T1,T2,T3>();
            return CreatContextFromConfiguration(configuration);
        }

        /// <summary>
        /// Creates an AggregateContext that is bound by the Type configured for it
        /// </summary>
        /// <typeparam name="T1">The Type to configure the context for</typeparam>
        /// <typeparam name="T2">The Type to configure the context for</typeparam>
        /// <typeparam name="T3">The Type to configure the context for</typeparam>
        /// <typeparam name="T4">The Type to configure the context for</typeparam>
        /// <returns><seealso cref="IDataContext"/>The IDataContext for usage</returns>
        public static IDataContext Create<T1, T2, T3, T4>()
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var configuration = AggregateConfigurationFactory.GetConfigurationFor<T1,T2,T3,T4>();
            return CreatContextFromConfiguration(configuration);
        }
    }
}
