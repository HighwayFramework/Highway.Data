using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using Ninject;

namespace Highway.Data.EntityFramework.Ninject
{
    public static class Bootstrapper
    {
        public static void BuildHighway(this IKernel kernel)
        {
            kernel.Bind<IEventManager>().To<EventManager>();
        }
    }
}