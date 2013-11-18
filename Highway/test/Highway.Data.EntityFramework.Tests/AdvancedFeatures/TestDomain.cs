using System;
using System.Collections.Generic;
using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.EntityFramework.Tests.EventManagement
{
    public class TestDomain : IDomain
    {
        public List<IInterceptor> Events { get; set; }

        //This is not here

        public string ConnectionString { get; set; }
        public IMappingConfiguration Mappings { get; set; }
        public IContextConfiguration Context { get; set; }
    }
}