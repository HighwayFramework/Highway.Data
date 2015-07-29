using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    /// Intercepts after the execute of a scalar
    /// </summary>
    public class AfterScalar : EventArgs
    {
        public object Query { get; set; }

        public AfterScalar(object query)
        {
            Query = query;
        }
    }
}