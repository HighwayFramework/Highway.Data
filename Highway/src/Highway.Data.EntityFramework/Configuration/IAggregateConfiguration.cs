using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data
{
    public interface IAggregateConfiguration
    {
        IEnumerable<Type> TypesConfigured { get; set; }
        string ConnectionString { get; set; }
        IMappingConfiguration[] Mappings { get; set; }
        ILog Logger { get; set; }
        IContextConfiguration ContextConfiguration { get; set; }
    }
}
