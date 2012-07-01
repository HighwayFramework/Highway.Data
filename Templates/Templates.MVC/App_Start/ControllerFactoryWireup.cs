using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Castle.Core.Logging;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Templates.App_Start.ControllerFactoryWireup), "PostStartup")]
namespace Templates.App_Start
{
    public static class ControllerFactoryWireup
    {
        public static void PostStartup()
        {
            ControllerBuilder.Current.SetControllerFactory(IoC.Container.Resolve<IControllerFactory>());
        }
    }
}