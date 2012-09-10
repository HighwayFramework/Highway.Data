using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data
{
    public class AggregateConfiguration : IAggregateConfiguration
    {
        public AggregateConfiguration(string connectionString, IMappingConfiguration[] mappings, ILog logger, IContextConfiguration contextConfiguration, params Type[] typesConfigured)
        {
            ConnectionString = connectionString;
            Mappings = mappings;
            Logger = logger ?? new NoOpLogger();
            ContextConfiguration = contextConfiguration ?? new DefaultContextConfiguration();
            TypesConfigured = typesConfigured;
        }

        public IEnumerable<Type> TypesConfigured { get; set; }

        public string ConnectionString { get; set; }

        public IMappingConfiguration[] Mappings { get; set; }

        public ILog Logger { get; set; }

        public IContextConfiguration ContextConfiguration { get; set; }
    }
}
