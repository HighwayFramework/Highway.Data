using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.CSharp;

namespace Highway.Data.EntityFramework.Builder
{
    /// <summary>
    /// Dynamicly creates subclasses and then stores that type by the configured aggregate root types for future instantiation.
    /// </summary>
    internal static class DynamicAggregateContextTypeBuilder
    {
        private static readonly ConcurrentDictionary<string, Type> PreviousContextTypes =
            new ConcurrentDictionary<string, Type>();

        /* BE VERY CAREFUL WITH THIS
        * HERE BE DRAGONS!!!
        * 
        * This is the class declaration that we compiled with a guid in the name
        * at runtime so that each aggregate root ends up a different model compilation
        * with entity framework, and then that type is cached. If you modify this and break
        * the function. Devlin will hunt you down and feed you to the dragon!!!
        * */
        private const string ClassString = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highway.Data
{
    public class AggregateContext{className} : AggregateDataContext
    {
        public AggregateContext{className}(IAggregateConfiguration configuration)
            : base(configuration)
        {

        }
    }
}
";

        private static readonly string[] ReferenceAssemblies = new[]
            {
                "System.dll",
                "System.Linq.dll",
                "EntityFramework.dll",
                "Highway.Data.dll",
                "Highway.Data.EntityFramework.dll"
            };

        private static string BuildKey(IAggregateConfiguration configuration)
        {
            return string.Join(",", configuration.TypesConfigured.Select(x => x.FullName));
        }

        /// <summary>
        /// Builds the type for the Context
        /// </summary>
        /// <param name="configuration">the aggregate configuration used to build the context</param>
        /// <returns>the IDataContext</returns>
        public static Type Build(IAggregateConfiguration configuration)
        {
            string key = BuildKey(configuration);

            if (PreviousContextTypes.ContainsKey(key))
            {
                return PreviousContextTypes[key];
            }

            Type contextType = ConstructNewContextType();
            PreviousContextTypes.AddOrUpdate(key, contextType, (k, existingValue) => contextType);
            return contextType;
        }

        private static Type ConstructNewContextType()
        {
            var codeDomProvider = new CSharpCodeProvider();

            Guid typeGuid = Guid.NewGuid();
            string typeGuidString =
                typeGuid.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
            string classDeclaration = ClassString.Replace("{className}", typeGuidString);
            string name = string.Format("Highway.Data.AggregateContext{0}", typeGuidString);
            var newCompilerParameters = new CompilerParameters(ReferenceAssemblies)
                {
                    GenerateInMemory = true,
                    MainClass = name
                };
            CompilerResults output = codeDomProvider.CompileAssemblyFromSource(newCompilerParameters, classDeclaration);
            Type type = output.CompiledAssembly.GetType(name);
            return type;
        }

       
    }
}