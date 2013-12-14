using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;

namespace Highway.Data
{
    public static class Mappings
    {
        /// <summary>
        /// Creates a mapping for the type provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TypeMappings FromAssemblyContaing<T>()
        {
            return new TypeMappings().FromAssemblyContaing<T>();
        }

        public static TypeMappings FromAssemblyContaing<T>(this TypeMappings mappings)
        {
            mappings.AddType<T>();
            return mappings;
        }

    }

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