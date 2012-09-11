using System;
using System.Data.Entity;
using System.Reflection;
using Highway.Data.EntityFramework.Builder;
using Highway.Data.Interfaces;

namespace Highway.Data
{
    /// <summary>
    /// Factory for creating aggregate root based bounded contexts
    /// </summary>
    public abstract class AggregateContextFactory
    {
        private static IDataContext CreatContextFromConfiguration(IAggregateConfiguration configuration)
        {
            Type newContextType = DynamicAggregateContextTypeBuilder.Build(configuration);
            SetInitialIzer(newContextType);
            var context = (IDataContext) Activator.CreateInstance(newContextType, new object[] {configuration});
            return context;
        }

        private static void SetInitialIzer(Type newContextType)
        {
            Type typeofClassWithGenericStaticMethod = typeof (Database);

            // Grabbing the specific static method
            MethodInfo methodInfo = typeofClassWithGenericStaticMethod.GetMethod("SetInitializer",
                                                                                 BindingFlags.Static |
                                                                                 BindingFlags.Public);

            // Binding the method info to generic arguments
            var genericArguments = new[] {newContextType};
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArguments);

            // Simply invoking the method and passing parameters
            // The null parameter is the object to call the method from. Since the method is
            // static, pass null.
            object returnValue = genericMethodInfo.Invoke(null, new object[] {null});
        }

        /// <summary>
        /// Creates an AggregateContext that is bound by the Type configured for it
        /// </summary>
        /// <typeparam name="T1">The Type to configure the context for</typeparam>
        /// <returns><seealso cref="IDataContext"/>The IDataContext for usage</returns>
        public static IDataContext Create<T1>()
            where T1 : class
        {
            IAggregateConfiguration configuration = AggregateConfigurationFactory.GetConfigurationFor<T1>();
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
            IAggregateConfiguration configuration = AggregateConfigurationFactory.GetConfigurationFor<T1, T2>();
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
            IAggregateConfiguration configuration = AggregateConfigurationFactory.GetConfigurationFor<T1, T2, T3>();
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
            IAggregateConfiguration configuration = AggregateConfigurationFactory.GetConfigurationFor<T1, T2, T3, T4>();
            return CreatContextFromConfiguration(configuration);
        }
    }
}