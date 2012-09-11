using Highway.Data.EventManagement;
using Highway.Data.Interfaces;
using Microsoft.Practices.Unity;

namespace Highway.Data.EntityFramework.Unity
{
    public static class Bootstrapper
    {
        public static void BuildHighway(this IUnityContainer container)
        {
            container.RegisterType<IEventManager, EventManager>();
        }
    }
}