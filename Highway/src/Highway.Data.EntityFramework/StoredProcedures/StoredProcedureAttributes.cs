#region

using System;
using System.Data;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     Contains attributes for Stored Procedure processing
    /// </summary>
    public class StoredProcedureAttributes
    {
        /// <summary>
        ///     Defines the direction of data flow for the property/parameter.
        /// </summary>
        public class Direction : Attribute
        {
            public Direction(ParameterDirection d)
            {
                Value = d;
            }

            public ParameterDirection Value { get; set; }
        }

        /// <summary>
        ///     Parameter name override. Default value for parameter name is the name of the
        ///     property. This overrides that default with a user defined name.
        /// </summary>
        public class Name : Attribute
        {
            public Name(String s)
            {
                Value = s;
            }

            public String Value { get; set; }
        }

        /// <summary>
        ///     Define the SqlDbType for the parameter corresponding to this property.
        /// </summary>
        public class ParameterType : Attribute
        {
            public ParameterType(SqlDbType t)
            {
                Value = t;
            }

            public SqlDbType Value { get; set; }
        }

        /// <summary>
        ///     Size in bytes of returned data. Should be used on output and returncode parameters.
        /// </summary>
        public class Precision : Attribute
        {
            public Precision(Byte s)
            {
                Value = s;
            }

            public Byte Value { get; set; }
        }

        /// <summary>
        ///     Size in bytes of returned data. Should be used on output and returncode parameters.
        /// </summary>
        public class Scale : Attribute
        {
            public Scale(Byte s)
            {
                Value = s;
            }

            public Byte Value { get; set; }
        }

        /// <summary>
        ///     Allows the setting of the user defined table type name for table valued parameters
        /// </summary>
        public class Schema : Attribute
        {
            public Schema(String t)
            {
                Value = t;
            }

            public String Value { get; set; }
        }

        /// <summary>
        ///     Size in bytes of returned data. Should be used on output and returncode parameters.
        /// </summary>
        public class Size : Attribute
        {
            public Size(Int32 s)
            {
                Value = s;
            }

            public Int32 Value { get; set; }
        }

        /// <summary>
        ///     Allows the setting of the user defined table type name for table valued parameters
        /// </summary>
        public class TableName : Attribute
        {
            public TableName(String t)
            {
                Value = t;
            }

            public String Value { get; set; }
        }

        /// <summary>
        ///     Allows the setting of the parameter type name for user defined types in the database
        /// </summary>
        public class TypeName : Attribute
        {
            public TypeName(String t)
            {
                Value = t;
            }

            public String Value { get; set; }
        }
    }
}