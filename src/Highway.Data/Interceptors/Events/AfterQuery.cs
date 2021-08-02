using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     The Event Arguments for a after Query execution Interceptor to use
    /// </summary>
    public class AfterQuery : EventArgs
    {
        public AfterQuery(object result)
        {
            Result = result;
        }

        public object Result { get; set; }
    }
}
