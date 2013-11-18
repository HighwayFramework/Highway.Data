#region

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.Windsor;

#endregion

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