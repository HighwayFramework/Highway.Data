using Microsoft.Practices.ServiceLocation;

namespace Highway.Data
{
    public abstract class AggregateConfigurationFactory
    {
        /// <summary>
        /// Resolves the configuration for the type from common service locator using named instances
        /// </summary>
        /// <typeparam name="T1">The aggregate root type that has been configured</typeparam>
        /// <returns>The configuration</returns>
        public static IAggregateConfiguration GetConfigurationFor<T1>() 
            where T1 : class
        {
            return ServiceLocator.Current.GetInstance<IAggregateConfiguration>(AggregateConfigurationKeyFactory.GenerateKey<T1>());
        }

        /// <summary>
        /// Resolves the configuration for the type from common service locator using named instances
        /// </summary>
        /// <typeparam name="T1">One of the aggregate root types that has been configured</typeparam>
        /// <typeparam name="T2">One of the aggregate root types that has been configured</typeparam>
        /// <returns>The configuration</returns>
        public static IAggregateConfiguration GetConfigurationFor<T1, T2>() 
            where T1 : class
            where T2 : class 
        {
            return
                ServiceLocator.Current.GetInstance<IAggregateConfiguration>(AggregateConfigurationKeyFactory.GenerateKey<T1,T2>());
        }

        /// <summary>
        /// Resolves the configuration for the type from common service locator using named instances
        /// </summary>
        /// <typeparam name="T1">One of the aggregate root types that has been configured</typeparam>
        /// <typeparam name="T2">One of the aggregate root types that has been configured</typeparam>
        /// <typeparam name="T3">One of the aggregate root types that has been configured</typeparam>
        /// <returns>The configuration</returns>
        public static IAggregateConfiguration GetConfigurationFor<T1, T2, T3>() 
            where T1 : class 
            where T2 : class 
            where T3 : class
        {
            return
                ServiceLocator.Current.GetInstance<IAggregateConfiguration>(AggregateConfigurationKeyFactory.GenerateKey<T1,T2,T3>());
        }

        /// <summary>
        /// Resolves the configuration for the type from common service locator using named instances
        /// </summary>
        /// <typeparam name="T1">One of the aggregate root types that has been configured</typeparam>
        /// <typeparam name="T2">One of the aggregate root types that has been configured</typeparam>
        /// <typeparam name="T3">One of the aggregate root types that has been configured</typeparam>
        /// <typeparam name="T4">One of the aggregate root types that has been configured</typeparam>
        /// <returns>The configuration</returns>
        public static IAggregateConfiguration GetConfigurationFor<T1, T2, T3, T4>() 
            where T1 : class 
            where T2 : class 
            where T3 : class 
            where T4 : class
        {
            return
                ServiceLocator.Current.GetInstance<IAggregateConfiguration>(AggregateConfigurationKeyFactory.GenerateKey<T1, T2, T3, T4>());
        }
    }
}