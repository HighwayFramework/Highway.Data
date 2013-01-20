using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace Highway.Data
{
    /// <summary>
    /// Contains extension methods to Code First database objects for Stored Procedure processing
    /// </summary>
    internal static class StoredProcedureHelpers
    {
        /// <summary>
        /// Get the underlying class type for lists, etc. that implement IEnumerable<>.
        /// </summary>
        /// <param name="listtype"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(Type listtype)
        {
            Type basetype = null;
            foreach (Type i in listtype.GetInterfaces())
                if (i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                    basetype = i.GetGenericArguments()[0];

            return basetype;
        }
        /// <summary>
        /// Get properties of a type that do not have the 'NotMapped' attribute
        /// </summary>
        /// <param name="t">Type to examine for properites</param>
        /// <returns>Array of properties that can be filled</returns>
        public static PropertyInfo[] GetMappedProperties(this Type t)
        {
            var props1 = t.GetProperties();
            var props2 = props1
                .Where(p => GetAttribute<NotMappedAttribute>((PropertyInfo) p) == null)
                .Select(p => p);
            return props2.ToArray();
        }

        /// <summary>
        /// Get an attribute for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo"></param>
        /// <param name="customAttribute"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type)
            where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return (T)attributes;
        }

        /// <summary>
        /// Get an attribute for a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo"></param>
        /// <param name="customAttribute"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this PropertyInfo propertyinfo)
            where T : Attribute
        {
            var attributes = propertyinfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return (T)attributes;
        }

        /// <summary>
        /// Read data for the current result row from a reader into a destination object, by the name
        /// of the properties on the destination object.
        /// </summary>
        /// <param name="reader">data reader holding return data</param>
        /// <param name="t">object to populate</param>
        /// <returns></returns>
        public static object ReadRecord(this DbDataReader reader, object t, PropertyInfo[] props)
        {
            String name;

            // copy mapped properties
            foreach (PropertyInfo p in props)
            {
                try
                {
                    // default name is property name, override of parameter name by attribute
                    var attr = p.GetAttribute<StoredProcedureAttributes.Name>();
                    name = (null == attr) ? p.Name : attr.Value;

                    // get the requested value from the returned dataset and handle null values
                    var data = reader[name];
                    if (data.GetType() == typeof(System.DBNull))
                        p.SetValue(t, null, null);
                    else
                        p.SetValue(t, reader[name], null);
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(IndexOutOfRangeException))
                    {
                        // if the result set doesn't have this value, intercept the exception
                        // and set the property value to null / 0
                        p.SetValue(t, null, null);
                    }
                    else
                        // something bad happened, pass on the exception
                        throw ex;
                }
            }

            return t;
        }

        /// <summary>
        /// Read data for the current result row from a reader into a destination object, by the name
        /// of the properties on the destination object.
        /// </summary>
        /// <param name="reader">data reader holding return data</param>
        /// <param name="t">object to populate</param>
        /// <returns></returns>
        public static object ReadRecord(this SqlDataReader reader, object t, PropertyInfo[] props)
        {
            String name = "";

            // copy mapped properties
            foreach (PropertyInfo p in props)
            {
                try
                {
                    // default name is property name, override of parameter name by attribute
                    var attr = p.GetAttribute<StoredProcedureAttributes.Name>();
                    name = (null == attr) ? p.Name : attr.Value;

                    // get the requested value from the returned dataset and handle null values
                    var data = reader[name];
                    if (data.GetType() == typeof(System.DBNull))
                        p.SetValue(t, null, null);
                    else
                        p.SetValue(t, reader[name], null);
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(IndexOutOfRangeException))
                    {
                        // if the result set doesn't have this value, intercept the exception
                        // and set the property value to null / 0
                        p.SetValue(t, null, null);
                    }
                    else
                    {
                        // tell the user *where* we had an exception
                        Exception outer = new Exception(String.Format("Exception processing return column {0} in {1}",
                                                                      name, t.GetType().Name), ex);

                        // something bad happened, pass on the exception
                        throw outer;
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// Do the work of converting a source data object to SqlDataRecords 
        /// using the parameter attributes to create the table valued parameter definition
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<SqlDataRecord> TableValuedParameter(IList table)
        {
            // get the object type underlying our table
            Type t = StoredProcedureHelpers.GetUnderlyingType(table.GetType());

            // list of converted values to be returned to the caller
            List<SqlDataRecord> recordlist = new List<SqlDataRecord>();

            // get all mapped properties
            PropertyInfo[] props = StoredProcedureHelpers.GetMappedProperties(t);

            // get the column definitions, into an array
            List<SqlMetaData> columnlist = new List<SqlMetaData>();

            // get the propery column name to property name mapping
            // and generate the SqlMetaData for each property/column
            Dictionary<String, String> mapping = new Dictionary<string, string>();
            foreach (PropertyInfo p in props)
            {
                // default name is property name, override of parameter name by attribute
                var attr = p.GetAttribute<StoredProcedureAttributes.Name>();
                String name = (null == attr) ? p.Name : attr.Value;
                mapping.Add(name, p.Name);

                // get column type
                var ct = p.GetAttribute<StoredProcedureAttributes.ParameterType>();
                SqlDbType coltype = (null == ct) ? SqlDbType.Int : ct.Value;

                // create metadata column definition
                SqlMetaData column;
                switch (coltype)
                {
                    case SqlDbType.Binary:
                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                    case SqlDbType.Image:
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Text:
                    case SqlDbType.NText:
                    case SqlDbType.VarBinary:
                        // get column size
                        var sa = p.GetAttribute<StoredProcedureAttributes.Size>();
                        int size = (null == sa) ? 50 : sa.Value;
                        column = new SqlMetaData(name, coltype, size);
                        break;

                    case SqlDbType.Decimal:
                        // get column precision and scale
                        var pa = p.GetAttribute<StoredProcedureAttributes.Precision>();
                        Byte precision = (null == pa) ? (byte)10 : pa.Value;
                        var sca = p.GetAttribute<StoredProcedureAttributes.Scale>();
                        Byte scale = (null == sca) ? (byte)2 : sca.Value;
                        column = new SqlMetaData(name, coltype, precision, scale);
                        break;

                    default:
                        column = new SqlMetaData(name, coltype);
                        break;
                }

                // Add metadata to column list
                columnlist.Add(column);
            }

            // load each object in the input data table into sql data records
            foreach (object s in table)
            {
                // create the sql data record using the column definition
                SqlDataRecord record = new SqlDataRecord(columnlist.ToArray());
                for (int i = 0; i < columnlist.Count(); i++)
                {
                    // locate the value of the matching property
                    var value = props.Where(p => p.Name == mapping[columnlist[i].Name])
                                     .First()
                                     .GetValue(s, null);

                    // set the value
                    record.SetValue(i, value);
                }

                // add the sql data record to our output list
                recordlist.Add(record);
            }

            // return our list of data records
            return recordlist;
        }
    }
}