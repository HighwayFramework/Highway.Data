using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.EntityFramework.Castle;
using Highway.Data.GettingStarted.DataAccess.Mappings;
using Highway.Data.GettingStarted.IoC.UI.Properties;
using Highway.Data.Interfaces;

namespace Highway.Data.GettingStarted.IoC.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WireUpIoC();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

        }

        private void WireUpIoC()
        {
            var bootstrap = new CastleBootstrap();
            var container = new WindsorContainer();
            bootstrap.Install(container, null);

            //Wiring in our types
            container.Register(
                Component.For<IDataContext>().ImplementedBy<DataContext>().DependsOn(Dependency.OnConfigValue("connectionString", Settings.Default.ConnectionString)),
                Component.For<IRepository>().ImplementedBy<Repository>(),
                Component.For<IMappingConfiguration>().ImplementedBy<GettingStartedMappings>(),
                Component.For<ILog>().ImplementedBy<NoOpLogger>(),
                Component.For<IContextConfiguration>().ImplementedBy<DefaultContextConfiguration>());
            

            //TODO: Wire up your types here

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }
    }


    public class WindsorControllerFactory : DefaultControllerFactory
    {
        readonly IWindsorContainer _container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            _container = container;

            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof (IController).IsAssignableFrom(t));
            
            foreach (Type t in controllerTypes)
            {
                _container.Register(Component.For(t).Named(t.FullName).LifestyleTransient());
            }


        }
        
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)_container.Resolve(controllerType);
        }
    }

}