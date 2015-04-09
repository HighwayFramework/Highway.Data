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
            // TODO Find the soft delete annotation and get the property name 
            //      Name of annotation will be something like:  
            //      http://schemas.microsoft.com/ado/2013/11/edm/customannotation:SoftDeleteColumnName 
              
            MetadataProperty annotation = type.MetadataProperties.SingleOrDefault(p => p.Name.EndsWith("customannotation:SoftDeleteColumnName"));
 
            return annotation == null ? null : (string)annotation.Value; 
        } 
    }
}