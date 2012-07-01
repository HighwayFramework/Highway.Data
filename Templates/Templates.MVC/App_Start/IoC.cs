using System;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel;
using Castle.Windsor.Installer;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Templates.App_Start.IoC), "Startup")]
namespace Templates.App_Start
{
    public static class IoC
    {
        public static IWindsorContainer Container { get; set; }

        public static void Startup()
        {
            // Create the container
            Container = new WindsorContainer();

            // Register the kernel, in case an installer needs it.
            Container.Register(Component.For<IKernel>().Instance(Container.Kernel));

            // Search for an use all installers in this application.
            Container.Install(FromAssembly.InThisApplication());
        }
    }
}