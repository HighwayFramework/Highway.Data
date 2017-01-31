using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Highway.Data.EntityFramework.Tests.UnitTests;
using Highway.Data.Tests.TestDomain;

namespace Highway.Data.EntityFramework.Tests.Initializer
{
    public class EntityFrameworkInitializer : DropCreateInitializer<TestDataContext>
    {
        public EntityFrameworkInitializer()
            : base(SeedDatabase, AdoCommands)
        {
        }

        private static IEnumerable<string> AdoCommands()
        {
            var assembly = typeof(EntityFrameworkInitializer).Assembly;
            var initializerScriptResourceNames =
                assembly.GetManifestResourceNames()
                    .Where(x => x.Contains("Highway.Data.EntityFramework.Tests.Initializer"))
                    .OrderBy(x => x);

            foreach (var resourceName in initializerScriptResourceNames)
            {
                using (var manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (manifestResourceStream == null)
                    {
                        continue;
                    }

                    using (var streamReader = new StreamReader(manifestResourceStream))
                    {
                        yield return streamReader.ReadToEnd();
                    }
                }
            }
        }

        private static void SeedDatabase(TestDataContext context)
        {
            for (var i = 0; i < 5; i++)
            {
                context.Add(new Foo());
            }

            context.SaveChanges();
        }
    }
}