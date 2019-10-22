
using System;
using System.Collections.Generic;


namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     The Event Arguments for a after Query execution Interceptor to use
    /// </summary>
    public class AfterQuery : EventArgs
    {
        public object Result { get; set; }

        public AfterQuery(object result)
        {
            Result = result;
        }
    }
}