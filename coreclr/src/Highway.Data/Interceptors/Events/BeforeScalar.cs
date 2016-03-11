using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    /// Intercepts before the scalar execute
    /// </summary>
    public class BeforeScalar : EventArgs
    {
        public object Query { get; set; }

        public BeforeScalar(object query)
        {
            Query = query;
        }
    }
}
