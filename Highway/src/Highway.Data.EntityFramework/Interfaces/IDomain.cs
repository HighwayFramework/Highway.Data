using System;
using System.Collections.Generic;
using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data
{
    public interface IDomain
    {
        string ConnectionString { get; set; }

        IMappingConfiguration Mappings { get; set; }

        IContextConfiguration Context { get; set; }

        List<IInterceptor> Events { get; set; }
    }
}