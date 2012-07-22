using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
