using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace Highway.Data.Tests
{
    public abstract class ContainerTest<T> : BaseTest<T> where T : class
    {
        public IWindsorContainer Container { get; set; }

        public virtual void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<T>().ImplementedBy<T>());
        }

        public virtual T ResolveTarget()
        {
            return Container.Resolve<T>();
        }

        public override void BeforeEachTest()
        {
            Container = new WindsorContainer();
            Container.Kernel.Resolver.AddSubResolver(new ArrayResolver(Container.Kernel));
            Container.AddFacility<TypedFactoryFacility>();
            RegisterComponents(Container);
            target = ResolveTarget();
            base.BeforeEachTest();
        }

        public override void AfterEachTest()
        {
            base.AfterEachTest();
            using (Container)
            {
            }
        }
    }
}
