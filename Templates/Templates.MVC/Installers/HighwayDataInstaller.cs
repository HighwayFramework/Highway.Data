using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Highway.Data.Interfaces;
using Highway.Data.EntityFramework.Repositories;
using Highway.Data.EventManagement;
using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;
using Templates.Models;
using System.Data.Entity;

namespace Templates.Installers
{
    // TODO Change the connection string to match your environment.
    public class HighwayDataInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDataContext>().ImplementedBy<EntityFrameworkContext>()
                    .DependsOn(new { connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ChangeMyConnectionString;Integrated Security=SSPI;" }),
                Component.For<IRepository>().ImplementedBy<EntityFrameworkRepository>(),
                Component.For<IEventManager>().ImplementedBy<EventManager>(),
                Component.For<IMappingConfiguration>().ImplementedBy<Mappings>(),
                Component.For<IDatabaseInitializer<EntityFrameworkContext>>().ImplementedBy<DatabaseInitializer>()
                );
        }
    }
}
