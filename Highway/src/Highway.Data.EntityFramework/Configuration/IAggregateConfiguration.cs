using System;
using System.Collections.Generic;
using Common.Logging;

namespace Highway.Data
{
    /// <summary>
    /// Configuration of an aggregate root context
    /// </summary>
    public interface IAggregateConfiguration
    {
        /// <summary>
        /// List of the types in the bounded context
        /// </summary>
        IEnumerable<Type> TypesConfigured { get; set; }
        
        /// <summary>
        /// Connection String for the bounded context
        /// </summary>
        string ConnectionString { get; set; }
        
        /// <summary>
        /// Mapping Configurations for the types in the bounded Context
        /// </summary>
        IMappingConfiguration[] Mappings { get; set; }
        
        /// <summary>
        /// Logged to be used by the bounded context
        /// </summary>
        ILog Logger { get; set; }
        
        /// <summary>
        /// Context level configuration for the bounded context
        /// </summary>
        IContextConfiguration ContextConfiguration { get; set; }
    }
}