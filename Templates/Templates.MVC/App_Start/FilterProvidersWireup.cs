using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Castle.Core.Logging;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Templates.App_Start.FilterProvidersWireup), "PostStartup")]
namespace Templates.App_Start
{
    public static class FilterProvidersWireup
    {
        private static ILogger logger = NullLogger.Instance;

        public static void PostStartup()
        {
#pragma warning disable 618
            var allProviders = IoC.Container.ResolveAll<IFilterProvider>();
#pragma warning restore 618
            foreach (var provider in allProviders)
            {
                logger.InfoFormat("Registering IFilterProvider : {0}", provider.GetType().Name);
                FilterProviders.Providers.Add(provider);
            }
        }
    }
}
