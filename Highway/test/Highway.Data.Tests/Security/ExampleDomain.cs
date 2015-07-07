using System.Collections.Generic;
using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.Tests.Security
{
    public class ExampleDomain : IDomain
    {
        public ExampleDomain()
        {
            Mappings = null;
        }
        public string ConnectionString { get; private set; }
        public IMappingConfiguration Mappings { get; private set; }
        public IContextConfiguration Context { get; private set; }
        public List<IInterceptor> Events { get; private set; }
    }
}