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
        string ConnectionString { get; set; }
        IMappingConfiguration[] Mappings { get; set; }
        ILog Logger { get; set; }
        IContextConfiguration ContextConfiguration { get; set; }
    }
    public interface IAggregateConfiguration<T1> : IAggregateConfiguration { }
    public interface IAggregateConfiguration<T1,T2> : IAggregateConfiguration { }
    public interface IAggregateConfiguration<T1,T2,T3> : IAggregateConfiguration { }
    public interface IAggregateConfiguration<T1,T2,T3,T4> : IAggregateConfiguration { }

}
