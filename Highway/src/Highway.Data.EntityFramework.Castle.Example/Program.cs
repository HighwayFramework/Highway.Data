using Highway.Data.Interfaces;
using System;

namespace Highway.Data.EntityFramework.Castle.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            const string SqlExpressConnectionString = @"Data Source=(local)\SQLExpress;Initial Catalog=HighwayDemo;Integrated Security=True";

            container.Register(Component.For<IRepository>().ImplementedBy<EntityFrameworkRepository>(),
                Component.For<IDataContext>().ImplementedBy<EntityFrameworkContext>().DependsOn(new { connectionString = SqlExpressConnectionString }),
	            Component.For<IEventHandler>().ImplementedBy<EventHandler>(),
                Component.For<IMappingConfiguration>().ImplementedBy<YourMappingClass>());

            // Use for Demos
            // DropCreateDatabaseIfModelChanges, Migrations not supported yet.  (IDatabaseInitializers)
            //Database.SetInitializer(new DropCreateDatabaseAlways<EntityFrameworkContext>());

            var application = ObjectFactory.GetInstance<DemoApplication>();
            application.Run();

            Console.Read();
        }
    }
}
