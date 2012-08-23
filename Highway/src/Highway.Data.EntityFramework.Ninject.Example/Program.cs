using Highway.Data.Interfaces;
using System;
using Ninject;

namespace Highway.Data.EntityFramework.Ninject.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            const string SqlExpressConnectionString = @"Data Source=(local)\SQLExpress;Initial Catalog=HighwayDemo;Integrated Security=True";

            var kernel = new StandardKernel();
            kernel.BuildHighway();
            kernel.Bind<IMappingConfiguration>().To<HighwayDataMappings>();
            kernel.Bind<IRepository>().To<Repository>();
            kernel.Bind<IDataContext>().To<DataContext>()
                .WithConstructorArgument("connectionString", SqlExpressConnectionString)
                .WithConstructorArgument("mapping", new HighwayDataMappings());
            kernel.Bind<DemoApplication>().To<DemoApplication>();



            // Use for Demos
            // DropCreateDatabaseIfModelChanges, Migrations not supported yet.  (IDatabaseInitializers)
            //Database.SetInitializer(new DropCreateDatabaseAlways<EntityFrameworkContext>());

            var application = kernel.Get<DemoApplication>();
            application.Run();

            Console.Read();
        }
    }
}
