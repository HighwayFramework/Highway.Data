using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace Highway.Data
{
    public class ConfigurationRegistrarAdapater : IConfigurationRegistrarAdapater
    {
        private readonly ConfigurationRegistrar _registrar;

        public ConfigurationRegistrarAdapater(ConfigurationRegistrar registrar)
        {
            _registrar = registrar;
        }

        /// <summary>
        /// Discovers all types that inherit from <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration"/> or
        ///             <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration"/> in the given assembly and adds an instance
        ///             of each discovered type to this registrar.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// Note that only types that are abstract or generic type definitions are skipped. Every
        ///             type that is discovered and added must provide a parameterless constructor.
        /// 
        /// </remarks>
        /// <param name="assembly">The assembly containing model configurations to add.</param>
        /// <returns>
        /// The same ConfigurationRegistrar instance so that multiple calls can be chained.
        /// </returns>
        public virtual ConfigurationRegistrarAdapater AddFromAssembly(Assembly assembly)
        {
            _registrar.AddFromAssembly(assembly);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration"/> to the <see cref="T:System.Data.Entity.DbModelBuilder"/>.
        ///             Only one <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration"/> can be added for each type in a model.
        /// 
        /// </summary>
        /// <typeparam name="TEntityType">The entity type being configured. </typeparam><param name="entityTypeConfiguration">The entity type configuration to be added. </param>
        /// <returns>
        /// The same ConfigurationRegistrar instance so that multiple calls can be chained.
        /// </returns>
        public new ConfigurationRegistrarAdapater Add<TEntityType>(EntityTypeConfiguration<TEntityType> entityTypeConfiguration) where TEntityType : class
        {
            _registrar.Add(entityTypeConfiguration);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration"/> to the <see cref="T:System.Data.Entity.DbModelBuilder"/>.
        ///             Only one <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration"/> can be added for each type in a model.
        /// 
        /// </summary>
        /// <typeparam name="TComplexType">The complex type being configured. </typeparam><param name="complexTypeConfiguration">The complex type configuration to be added </param>
        /// <returns>
        /// The same ConfigurationRegistrar instance so that multiple calls can be chained.
        /// </returns>
        public new ConfigurationRegistrarAdapater Add<TComplexType>(ComplexTypeConfiguration<TComplexType> complexTypeConfiguration) where TComplexType : class
        {
            _registrar.Add(complexTypeConfiguration);
            return this;

        }


        public new Type GetType()
        {
            return base.GetType();
        }
    }
}