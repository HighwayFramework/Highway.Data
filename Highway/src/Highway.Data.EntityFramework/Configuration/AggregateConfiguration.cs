using System;
using System.Collections.Generic;
using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data
{
    /// <summary>
    /// Base implementation of an Aggregate Root Bounded Context Configuration
    /// </summary>
    public class AggregateConfiguration : IAggregateConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="mappings"></param>
        /// <param name="logger"></param>
        /// <param name="contextConfiguration"></param>
        /// <param name="typesConfigured"></param>
        public AggregateConfiguration(string connectionString, IMappingConfiguration[] mappings, ILog logger,
                                      IContextConfiguration contextConfiguration, params Type[] typesConfigured)
        {
            ConnectionString = connectionString;
            Mappings = mappings;
            Logger = logger ?? new NoOpLogger();
            ContextConfiguration = contextConfiguration ?? new DefaultContextConfiguration();
            TypesConfigured = typesConfigured;
        }

        #region IAggregateConfiguration Members

        /// <summary>
        /// List of the types in the bounded context
        /// </summary>
        public IEnumerable<Type> TypesConfigured { get; set; }

        /// <summary>
        /// Connection String for the bounded context
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Mapping Configurations for the types in the bounded Context
        /// </summary>
        public IMappingConfiguration[] Mappings { get; set; }

        /// <summary>
        /// Logged to be used by the bounded context
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// Context level configuration for the bounded context
        /// </summary>
        public IContextConfiguration ContextConfiguration { get; set; }

        #endregion
    }
}