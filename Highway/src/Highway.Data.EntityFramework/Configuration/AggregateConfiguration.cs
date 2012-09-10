using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data
{
    public class AggregateConfiguration<T1> : IAggregateConfiguration
    {
        public AggregateConfiguration(string connectionString, IMappingConfiguration[] mappings, ILog logger, IContextConfiguration contextConfiguration)
        {
            ConnectionString = connectionString;
            Mappings = mappings;
            Logger = logger ?? new NoOpLogger();
            ContextConfiguration = contextConfiguration;
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration[] Mappings { get; set; }

        public ILog Logger { get; set; }

        public IContextConfiguration ContextConfiguration { get; set; }
    }


    public class AggregateConfiguration<T1, T2> : IAggregateConfiguration
    {
        public AggregateConfiguration(string connectionString, IMappingConfiguration[] mappings, ILog logger, IContextConfiguration contextConfiguration)
        {
            ConnectionString = connectionString;
            Mappings = mappings;
            Logger = logger ?? new NoOpLogger();
            ContextConfiguration = contextConfiguration;
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration[] Mappings { get; set; }

        public ILog Logger { get; set; }

        public IContextConfiguration ContextConfiguration { get; set; }
    }

    public class AggregateConfiguration<T1, T2, T3> : IAggregateConfiguration
    {
        public AggregateConfiguration(string connectionString, IMappingConfiguration[] mappings, ILog logger, IContextConfiguration contextConfiguration)
        {
            ConnectionString = connectionString;
            Mappings = mappings;
            Logger = logger ?? new NoOpLogger();
            ContextConfiguration = contextConfiguration;
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration[] Mappings { get; set; }

        public ILog Logger { get; set; }

        public IContextConfiguration ContextConfiguration { get; set; }
    }

    public class AggregateConfiguration<T1, T2, T3, T4> : IAggregateConfiguration
    {
        public AggregateConfiguration(string connectionString, IMappingConfiguration[] mappings, ILog logger, IContextConfiguration contextConfiguration)
        {
            ConnectionString = connectionString;
            Mappings = mappings;
            Logger = logger ?? new NoOpLogger();
            ContextConfiguration = contextConfiguration;
        }

        public string ConnectionString { get; set; }

        public IMappingConfiguration[] Mappings { get; set; }

        public ILog Logger { get; set; }

        public IContextConfiguration ContextConfiguration { get; set; }
    }

}
