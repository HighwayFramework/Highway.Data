using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Highway.Data.EntityFramework.Interceptors.SoftDelete
{
    public class SoftDeleteAttribute : Attribute 
    { 
        public SoftDeleteAttribute(string column) 
        { 
            ColumnName = column; 
        } 
 
 
        public string ColumnName { get; set; } 
 
 
        public static string GetSoftDeleteColumnName(EdmType type) 
        { 
            MetadataProperty annotation = type.MetadataProperties.SingleOrDefault(p => p.Name.EndsWith("customannotation:SoftDeleteColumnName"));
 
            return annotation == null ? null : (string)annotation.Value; 
        } 
    }
}