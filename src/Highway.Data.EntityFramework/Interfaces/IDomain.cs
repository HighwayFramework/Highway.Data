using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data
{
    public interface IDomain
    {
        string ConnectionString { get; }

        IContextConfiguration Context { get; }

        List<IInterceptor> Events { get; }

        IMappingConfiguration Mappings { get; }
    }
}
