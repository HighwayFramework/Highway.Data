using Highway.Data.EntityFramework.Contexts;
using Highway.Data.EntityFramework.Mappings;
using Highway.Data.EntityFramework.Repositories;
using Highway.Data.Interfaces;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.EntityFramework.StructureMap.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            const string SqlExpressConnectionString = @"Data Source=(local)\SQLExpress;Initial Catalog=HighwayDemo;Integrated Security=True";

            ObjectFactory.Initialize(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.AssembliesFromApplicationBaseDirectory();

                x.For<IMappingConfiguration>().Use<HighwayDataMappings>();
                x.For<IRepository>().Use<Repository>();
                x.For<IDataContext>().Use<Context>()
                    .Ctor<string>("connectionString").Is(SqlExpressConnectionString)
                    .Ctor<IMappingConfiguration[]>("configurations").Is(new[] { new HighwayDataMappings() });

            }));

            // Use for Demos
            // DropCreateDatabaseIfModelChanges, Migrations not supported yet.  (IDatabaseInitializers)
            //Database.SetInitializer(new DropCreateDatabaseAlways<EntityFrameworkContext>());

            var application = ObjectFactory.GetInstance<DemoApplication>();
            application.Run();

            Console.Read();
        }
    }
}
