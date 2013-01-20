using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection;

namespace Highway.Data
{
    /// <summary>
    /// Contains extension methods to Code First database objects for Stored Procedure processing
    /// </summary>
    public static class StoredProcedures
    {
        /// <summary>
        /// Generic Typed version of calling a stored procedure
        /// </summary>
        /// <typeparam name="T">Type of object containing the parameter data</typeparam>
        /// <param name="context">Database Context to use for the call</param>
        /// <param name="procedure">Generic Typed stored procedure object</param>
        /// <param name="data">The actual object containing the parameter data</param>
        /// <returns></returns>
        public static ResultsList CallStoredProc<T>(this DbContext context, StoredProc<T> procedure, T data)
        {
            IEnumerable<SqlParameter> parms = procedure.Parameters(data);
            ResultsList results = context.ReadFromStoredProc(procedure.fullname, parms, procedure.returntypes);
            procedure.ProcessOutputParms(parms, data);
            return results ?? new ResultsList();
        }

        /// <summary>
        /// Call a stored procedure, passing in the stored procedure object and a list of parameters
        /// </summary>
        /// <param name="context">Database context used for the call</param>
        /// <param name="procedure">Stored Procedure</param>
        /// <param name="parms">List of parameters</param>
        /// <returns></returns>
        public static ResultsList CallStoredProc(this DbContext context, StoredProc procedure, IEnumerable<SqlParameter> parms = null)
        {
            ResultsList results = context.ReadFromStoredProc(procedure.fullname, parms, procedure.returntypes);
            return results ?? new ResultsList();
        }

        /// <summary>
        /// internal
        /// 
        /// Call a stored procedure and get results back. 
        /// </summary>
        /// <param name="context">Code First database context object</param>
        /// <param name="tablename">Qualified name of proc to call</param>
        /// <param name="parms">List of ParameterHolder objects - input and output parameters</param>
        /// <param name="outputtypes">List of types to expect in return. Each type *must* have a default constructor.</param>
        /// <returns></returns>
        internal static ResultsList ReadFromStoredProc(this DbContext context,
                                                       String procname,
                                                       IEnumerable<SqlParameter> parms = null,
                                                       params Type[] outputtypes)
        {
            // create our output set object
            ResultsList results = new ResultsList();

            // ensure that we have a type list, even if it's empty
            IEnumerator currenttype = (null == outputtypes) ?
                                          new Type[0].GetEnumerator() :
                                          outputtypes.GetEnumerator();

            // handle to the database connection object
            var connection = (SqlConnection)context.Database.Connection;
            try
            {
                // open the connect for use and create a command object
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    // command to execute is our stored procedure
                    cmd.CommandText = procname;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // move parameters to command object
                    if (null != parms)
                        foreach (SqlParameter p in parms)
                            cmd.Parameters.Add(p);
                    //    foreach (ParameterHolder p in parms)
                    //        cmd.Parameters.Add(p.toParameter(cmd));

                    // Do It! This actually makes the database call
                    var reader = cmd.ExecuteReader();

                    // get the type we're expecting for the first result. If no types specified,
                    // ignore all results
                    if (currenttype.MoveNext())
                    {
                        // process results - repeat this loop for each result set returned by the stored proc
                        // for which we have a result type specified
                        do
                        {
                            // get properties to save for the current destination type
                            PropertyInfo[] props = ((Type)currenttype.Current).GetMappedProperties();

                            // create a destination for our results
                            List<object> current = new List<object>();

                            // process the result set
                            while (reader.Read())
                            {
                                // create an object to hold this result
                                object item = ((Type)currenttype.Current).GetConstructor(System.Type.EmptyTypes).Invoke(new object[0]);

                                // copy data elements by parameter name from result to destination object
                                reader.ReadRecord(item, props);

                                // add newly populated item to our output list
                                current.Add(item);
                            }

                            // add this result set to our return list
                            results.Add(current);
                        }
                        while (reader.NextResult() && currenttype.MoveNext());
                    }
                    // close up the reader, we're done saving results
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from stored proc " + procname + ": " + ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return results;
        }
    }
}