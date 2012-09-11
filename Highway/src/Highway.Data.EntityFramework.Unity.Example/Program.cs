using System;
using Highway.Data.Interfaces;
using Microsoft.Practices.Unity;

namespace Highway.Data.EntityFramework.Unity.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string SqlExpressConnectionString =
                @"Data Source=(local)\SQLExpress;Initial Catalog=HighwayDemo;Integrated Security=True";

            var unityContainer = new UnityContainer();
            unityContainer.BuildHighway();
            unityContainer.RegisterType<IMappingConfiguration, HighwayDataMappings>();
            unityContainer.RegisterType<IRepository, Repository>();
            unityContainer.RegisterType<IDataContext, DataContext>(new InjectionConstructor(SqlExpressConnectionString,
                                                                                            new HighwayDataMappings()));


            // Use for Demos
            // DropCreateDatabaseIfModelChanges, Migrations not supported yet.  (IDatabaseInitializers)
            //Database.SetInitializer(new DropCreateDatabaseAlways<EntityFrameworkContext>());

            var application = unityContainer.Resolve<DemoApplication>();
            application.Run();

            Console.Read();
        }
    }
}