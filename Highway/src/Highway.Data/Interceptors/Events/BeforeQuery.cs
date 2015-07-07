
using System;


namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     The Event Arguments for a post query execution Interceptor to use
    /// </summary>
    public class BeforeQuery : EventArgs
    {
        public object Query { get; set; }

        public BeforeQuery(object query)
        {
            Query = query;
        }
    }
}