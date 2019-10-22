using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;

namespace Highway.Data
{
    /// <summary>
    /// Mappings for the assembly containing the type provided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeMappings : IMappingConfiguration
    {
        private List<Type> _types = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            var added = new List<Assembly>();
            foreach (var type in _types)
            {
                if(added.Contains(type.Assembly)) continue;
                modelBuilder.Configurations.AddFromAssembly(type.Assembly);    
                added.Add(type.Assembly);
            }
            added = null;
            _types = null;
        }

        public void AddType<T>()
        {
            _types.Add(typeof(T));
        }
    }
}