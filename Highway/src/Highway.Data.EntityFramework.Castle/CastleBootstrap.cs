using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Repositories;
using Highway.Data.EventManagement;
using Highway.Data.Interfaces;

namespace Highway.Data.EntityFramework.Castle
{
    /// <summary>
    /// Castle specific bootstrap for installing types needed for usage to the current container
    /// </summary>
    public class CastleBootstrap : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IEventManager>().ImplementedBy<EventManager>()
                );
        }
    }
}
