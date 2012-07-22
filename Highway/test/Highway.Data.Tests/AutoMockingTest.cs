using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;

namespace Highway.Data.Tests
{
    public class AutoMockingTest<T> : ContainerTest<T> where T : class
    {
        public override void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<ILazyComponentLoader>().ImplementedBy<AutoMockingLazyComponentLoader>());
            base.RegisterComponents(container);
        }
    }
}
