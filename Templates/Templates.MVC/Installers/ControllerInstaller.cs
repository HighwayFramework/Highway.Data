using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace Templates.Installers
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly()
                    .BasedOn<IController>()
                    .LifestyleTransient(),
                Component.For<IControllerFactory>()
                    .ImplementedBy<WindsorControllerFactory>()
                    .LifestyleSingleton()
                );
        }
    }
}