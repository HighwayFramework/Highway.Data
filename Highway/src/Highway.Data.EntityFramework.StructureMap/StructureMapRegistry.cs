using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.EntityFramework.StructureMap
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IEventManager>().Use<EventManager>();
        }
    }
}