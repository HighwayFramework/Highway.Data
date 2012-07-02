using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Web.Mvc;
using Templates.Filters;

namespace Templates.Installers
{
    public class FilterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IFilterProvider>().ImplementedBy<IoCFilterProvider>(),
                Component.For<ExceptionLoggingFilter>().ImplementedBy<ExceptionLoggingFilter>(),
                Component.For<Func<ControllerContext,ActionDescriptor,Filter>>().Instance(
                    (c,a) => new Filter(container.Resolve<ExceptionLoggingFilter>(), FilterScope.Last, int.MinValue))
                );
        }
    }
}
