using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;

namespace Highway.Data.EntityFramework.Builder
{
    public static class DynamicAggregateContextBuilder
    {
        private static Dictionary<string, Type> _previousContextTypes = new Dictionary<string, Type>();
        private static string BuildKey(IAggregateConfiguration configuration)
        {
            return string.Join(",", configuration.TypesConfigured.Select(x=>x.FullName));
        }
        public static Type Create(IAggregateConfiguration configuration)
        {
            string key = BuildKey(configuration);
            if(_previousContextTypes.ContainsKey(key))
            {
                return _previousContextTypes[key];
            }
            Type contextType = ConstructNewContextType();
            _previousContextTypes.Add(key, contextType);
            return contextType;
        }

        private static Type ConstructNewContextType()
        {
            var codeDomProvider = new Microsoft.CSharp.CSharpCodeProvider();
            Guid typeGuid = Guid.NewGuid();
            var classDeclaration = string.Format(classString, typeGuid);
            CompilerParameters newCompilerParameters = new CompilerParameters();
            var output = codeDomProvider.CompileAssemblyFromSource(newCompilerParameters, classDeclaration);
            var type = output.CompiledAssembly.GetType(string.Format("{0}AggregateContext",typeGuid));
            return type;
        }

        /*BE VERY CAREFUL WITH THIS
         * HERE BE DRAGONS!!!
         * 
         * This is the class declaration that we compiled with a guid in the name
         * at runtime so that each aggregate root ends up a different model compilation
         * with entity framework, and then that type is cached. If you modify this and break
         * the function. Devlin will hunt you down and feed you to the dragon!!!
         * */
        private static string classString = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highway.Data
{
    internal class {0}AggregateContext : AggregateDataContext
    {
        public {0}AggregateContext(IAggregateConfiguration configuration)
            : base(configuration)
        {

        }
    }
}
";

    }
}
