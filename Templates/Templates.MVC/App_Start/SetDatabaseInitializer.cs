using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Templates.Models;
using Highway.Data.EntityFramework.Contexts;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Templates.App_Start.SetDatabaseInitializer), "PostStartup")]
namespace Templates.App_Start
{
    public static class SetDatabaseInitializer
    {
        public static void PostStartup()
        {
            Database.SetInitializer(IoC.Container.Resolve<IDatabaseInitializer<EntityFrameworkContext>>());
        }
    }
}
