using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     The Event Arguments for a post query execution Interceptor to use
    /// </summary>
    public class BeforeQuery : EventArgs
    {
        public BeforeQuery(object query)
        {
            Query = query;
        }

        public object Query { get; set; }
    }
}
