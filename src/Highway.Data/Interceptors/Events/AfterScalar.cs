using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     Intercepts after the execute of a scalar
    /// </summary>
    public class AfterScalar : EventArgs
    {
        public AfterScalar(object query)
        {
            Query = query;
        }

        public object Query { get; set; }
    }
}
