using System;
using System.Data;

namespace Highway.Data
{
    /// <summary>
    /// Contains attributes for Stored Procedure processing
    /// </summary>
    public class StoredProcedureAttributes
    {
        /// <summary>
        /// Parameter name override. Default value for parameter name is the name of the 
        /// property. This overrides that default with a user defined name.
        /// </summary>
        public class Name : Attribute
        {
            public String Value { get; set; }

            public Name(String s)
                : base()
            {
                Value = s;
            }
        }

        /// <summary>
        /// Size in bytes of returned data. Should be used on output and returncode parameters.
        /// </summary>
        public class Size : Attribute
        {
            public Int32 Value { get; set; }

            public Size(Int32 s)
                : base()
            {
                Value = s;
            }
        }

        /// <summary>
        /// Size in bytes of returned data. Should be used on output and returncode parameters.
        /// </summary>
        public class Precision : Attribute
        {
            public Byte Value { get; set; }

            public Precision(Byte s)
                : base()
            {
                Value = s;
            }
        }

        /// <summary>
        /// Size in bytes of returned data. Should be used on output and returncode parameters.
        /// </summary>
        public class Scale : Attribute
        {
            public Byte Value { get; set; }

            public Scale(Byte s)
                : base()
            {
                Value = s;
            }
        }

        /// <summary>
        /// Defines the direction of data flow for the property/parameter.
        /// </summary>
        public class Direction : Attribute
        {
            public ParameterDirection Value { get; set; }

            public Direction(ParameterDirection d)
            {
                Value = d;
            }
        }

        /// <summary>
        /// Define the SqlDbType for the parameter corresponding to this property.
        /// </summary>
        public class ParameterType : Attribute
        {
            public SqlDbType Value { get; set; }

            public ParameterType(SqlDbType t)
            {
                Value = t;
            }
        }

        /// <summary>
        /// Allows the setting of the parameter type name for user defined types in the database
        /// </summary>
        public class TypeName : Attribute
        {
            public String Value { get; set; }

            public TypeName(String t)
            {
                Value = t;
            }
        }

        /// <summary>
        /// Allows the setting of the user defined table type name for table valued parameters
        /// </summary>
        public class TableName : Attribute
        {
            public String Value { get; set; }

            public TableName(String t)
            {
                Value = t;
            }
        }

        /// <summary>
        /// Allows the setting of the user defined table type name for table valued parameters
        /// </summary>
        public class Schema : Attribute
        {
            public String Value { get; set; }

            public Schema(String t)
            {
                Value = t;
            }
        }
    }
}