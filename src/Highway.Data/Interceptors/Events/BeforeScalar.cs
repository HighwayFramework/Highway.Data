﻿using System;

namespace Highway.Data.Interceptors.Events
{
    /// <summary>
    ///     Intercepts before the scalar execute
    /// </summary>
    public class BeforeScalar : EventArgs
    {
        public BeforeScalar(object query)
        {
            Query = query;
        }

        public object Query { get; set; }
    }
}
