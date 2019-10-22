using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Highway.Data
{
    /// <summary>
    /// Holds multiple Result Sets returned from a Stored Procedure call. 
    /// </summary>
    public class ResultsList : IEnumerable
    {
        // our internal object that is the list of results lists
        List<List<object>> thelist = new List<List<object>>();

        /// <summary>
        /// Add a results list to the results set
        /// </summary>
        /// <param name="list"></param>
        public void Add(List<object> list)
        {
            thelist.Add(list);
        }

        /// <summary>
        /// Return an enumerator over the internal list
        /// </summary>
        /// <returns>Enumerator over List<object> that make up the result sets </returns>
        public IEnumerator GetEnumerator()
        {
            return thelist.GetEnumerator();
        }

        /// <summary>
        /// Return the count of result sets
        /// </summary>
        public Int32 Count
        {
            get { return thelist.Count; }
        }

        /// <summary>
        /// Get the nth results list item
        /// </summary>
        /// <param name="index"></param>
        /// <returns>List of objects that make up the result set</returns>
        public List<object> this[int index]
        {
            get { return thelist[index]; }
        }

        /// <summary>
        /// Return the result set that contains a particular type and does a cast to that type.
        /// </summary>
        /// <typeparam name="T">Type that was listed in StoredProc object as a possible return type for the stored procedure</typeparam>
        /// <returns>List of T; if no results match, returns an empty list</returns>
        public List<T> ToList<T>()
        {
            // search each non-empty results list 
            foreach (List<object> list in thelist.Where(p => p.Count > 0).Select(p => p))
            {
                // compare types of the first element - this is why we filter for non-empty results
                if (typeof(T) == list[0].GetType())
                {
                    // do cast to return type
                    return list.Cast<T>().Select(p => p).ToList();
                }
            }

            // no matches? return empty list
            return new List<T>();
        }

        /// <summary>
        /// Return the result set that contains a particular type and does a cast to that type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Array of T; if no results match, returns an empty array</returns>
        public T[] ToArray<T>()
        {
            // search each non-empty results list 
            foreach (List<object> list in thelist.Where(p => p.Count > 0).Select(p => p))
            {
                // compare types of the first element - this is why we filter for non-empty results
                if (typeof(T) == list[0].GetType())
                {
                    // do cast to return type
                    return list.Cast<T>().Select(p => p).ToArray();
                }
            }

            // no matches? return empty array
            return new T[0];
        }
    }

    /// <summary>
    /// Genericized version of StoredProc object, takes a .Net POCO object type for the parameters. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StoredProc<T> : StoredProc
    {
        /// <summary>
        /// Constructor. Note that the return type objects must have a default constructor!
        /// </summary>
        /// <param name="types">Types returned by the stored procedure. Order is important!</param>
        public StoredProc(params Type[] types)
        {
            // set default schema
            schema = "dbo";

            // allow override by attribute
            var schema_attr = typeof(T).GetAttribute<StoredProcedureAttributes.Schema>();
            if (null != schema_attr)
                schema = schema_attr.Value;

            // set default proc name
            procname = typeof(T).Name;

            // allow override by attribute
            var procname_attr = typeof(T).GetAttribute<StoredProcedureAttributes.Name>();
            if (null != procname_attr)
                procname = procname_attr.Value;

            outputtypes.AddRange(types);
        }

        /// <summary>
        /// Contains a mapping of property names to parameter names. We do this since this mapping is complex; 
        /// i.e. the default parameter name may be overridden by the Name attribute
        /// </summary>
        internal Dictionary<String, String> MappedParams = new Dictionary<string, string>();

        /// <summary>
        /// Store output parameter values back into the data object
        /// </summary>
        /// <param name="parms">List of parameters</param>
        /// <param name="data">Source data object</param>
        internal void ProcessOutputParms(IEnumerable<SqlParameter> parms, T data)
        {
            // get the list of properties for this type
            PropertyInfo[] props = typeof(T).GetMappedProperties();

            // we want to write data back to properties for every non-input only parameter
            foreach (SqlParameter parm in parms
                .Where(p => p.Direction != ParameterDirection.Input)
                .Select(p => p))
            {
                // get the property name mapped to this parameter
                String propname = MappedParams.Where(p => p.Key == parm.ParameterName).Select(p => p.Value).First();

                // extract the matchingproperty and set its value
                PropertyInfo prop = props.Where(p => p.Name == propname).FirstOrDefault();
                prop.SetValue(data, parm.Value, null);
            }
        }

        /// <summary>
        /// Convert parameters from type T properties to SqlParameters
        /// </summary>
        /// <param name="data">Source data object</param>
        /// <returns></returns>
        internal IEnumerable<SqlParameter> Parameters(T data)
        {
            // clear the parameter to property mapping since we'll be recreating this
            MappedParams.Clear();

            // list of parameters we'll be returning
            List<SqlParameter> parms = new List<SqlParameter>();

            // properties that we're converting to parameters are everything without
            // a NotMapped attribute
            foreach (PropertyInfo p in typeof(T).GetMappedProperties())
            {
                //---------------------------------------------------------------------------------
                // process attributes
                //---------------------------------------------------------------------------------

                // create parameter and store default name - property name
                SqlParameter holder = new SqlParameter()
                {
                    ParameterName = p.Name
                };

                // override of parameter name by attribute
                var name = p.GetAttribute<StoredProcedureAttributes.Name>();
                if (null != name)
                    holder.ParameterName = name.Value;

                // save direction (default is input)
                var dir = p.GetAttribute<StoredProcedureAttributes.Direction>();
                if (null != dir)
                    holder.Direction = dir.Value;

                // save size
                var size = p.GetAttribute<StoredProcedureAttributes.Size>();
                if (null != size)
                    holder.Size = size.Value;

                // save database type of parameter
                var parmtype = p.GetAttribute<StoredProcedureAttributes.ParameterType>();
                if (null != parmtype)
                    holder.SqlDbType = parmtype.Value;

                // save user-defined type name
                var typename = p.GetAttribute<StoredProcedureAttributes.TypeName>();
                if (null != typename)
                    holder.TypeName = typename.Value;

                // save precision
                var precision = p.GetAttribute<StoredProcedureAttributes.Precision>();
                if (null != precision)
                    holder.Precision = precision.Value;

                // save scale
                var scale = p.GetAttribute<StoredProcedureAttributes.Scale>();
                if (null != scale)
                    holder.Scale = scale.Value;

                //---------------------------------------------------------------------------------
                // Save parameter value
                //---------------------------------------------------------------------------------

                // store table values, scalar value or null
                var value = p.GetValue(data, null);
                if (value == null)
                {
                    // set database null marker for null value
                    holder.Value = DBNull.Value;
                }
                else if (SqlDbType.Structured == holder.SqlDbType)
                {
                    // catcher - tvp must be ienumerable type
                    if (!(value is IEnumerable))
                        throw new InvalidCastException(String.Format("{0} must be an IEnumerable Type", p.Name));

                    // ge the type underlying the IEnumerable
                    Type basetype = StoredProcedureHelpers.GetUnderlyingType(value.GetType());

                    // get the table valued parameter table type name
                    var schema = p.GetAttribute<StoredProcedureAttributes.Schema>();
                    if (null == schema && null != basetype)
                        schema = basetype.GetAttribute<StoredProcedureAttributes.Schema>();

                    var tvpname = p.GetAttribute<StoredProcedureAttributes.TableName>();
                    if (null == tvpname && null != basetype)
                        tvpname = basetype.GetAttribute<StoredProcedureAttributes.TableName>();

                    holder.TypeName = (null != schema) ? schema.Value : "dbo";
                    holder.TypeName += ".";
                    holder.TypeName += (null != tvpname) ? tvpname.Value : p.Name;

                    // generate table valued parameter
                    holder.Value = StoredProcedureHelpers.TableValuedParameter((IList)value);
                }
                else
                {
                    // process normal scalar value
                    holder.Value = value;
                }

                // save the mapping between the parameter name and property name, since the parameter
                // name can be overridden
                MappedParams.Add(holder.ParameterName, p.Name);

                // add parameter to list
                parms.Add(holder);
            }

            return parms;
        }

        /// <summary>
        /// Fluent API - assign owner (schema)
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public new StoredProc<T> HasOwner(String owner)
        {
            base.HasOwner(owner);
            return this;
        }

        /// <summary>
        /// Fluent API - assign procedure name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new StoredProc<T> HasName(String name)
        {
            base.HasName(name);
            return this;
        }

        /// <summary>
        /// Fluent API - set the data types of resultsets returned by the stored procedure. 
        /// Order is important! Note that the return type objects must have a default constructor!
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public new StoredProc<T> ReturnsTypes(params Type[] types)
        {
            base.ReturnsTypes(types);
            return this;
        }
    }

    /// <summary>
    /// Represents a Stored Procedure in the database. Note that the return type objects
    /// must have a default constructor!
    /// </summary>
    public class StoredProc
    {
        /// <summary>
        /// Database owner of this object
        /// </summary>
        public String schema { get; set; }

        /// <summary>
        /// Name of the stored procedure
        /// </summary>
        public String procname { get; set; }

        /// <summary>
        /// Fluent API - assign owner (schema)
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public StoredProc HasOwner(String owner)
        {
            schema = owner;
            return this;
        }

        /// <summary>
        /// Fluent API - assign procedure name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StoredProc HasName(String name)
        {
            procname = name;
            return this;
        }

        /// <summary>
        /// Fluent API - set the data types of resultsets returned by the stored procedure. 
        /// Order is important! Note that the return type objects must have a default constructor!
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public StoredProc ReturnsTypes(params Type[] types)
        {
            outputtypes.AddRange(types);
            return this;
        }

        /// <summary>
        /// Get the fully (schema plus owner) name of the stored procedure
        /// </summary>
        internal String fullname
        {
            get { return schema + "." + procname; }
        }

        /// <summary>
        /// Constructors. Note that the return type objects
        /// must have a default constructor!
        /// </summary>
        public StoredProc()
        {
            schema = "dbo";
        }

        public StoredProc(String name)
        {
            schema = "dbo";
            procname = name;
        }

        public StoredProc(String name, params Type[] types)
        {
            schema = "dbo";
            procname = name;
            outputtypes.AddRange(types);
        }

        public StoredProc(String owner, String name, params Type[] types)
        {
            schema = owner;
            procname = name;
            outputtypes.AddRange(types);
        }

        /// <summary>
        /// List of data types that this stored procedure returns as result sets. 
        /// Order is important!
        /// </summary>
        internal List<Type> outputtypes = new List<Type>();

        /// <summary>
        /// Get an array of types returned
        /// </summary>
        internal Type[] returntypes
        {
            get { return outputtypes.ToArray(); }
        }
    }
}