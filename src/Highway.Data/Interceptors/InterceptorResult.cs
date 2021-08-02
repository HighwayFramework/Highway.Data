// <copyright file="InterceptorResult.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

namespace Highway.Data.Interceptors
{
    /// <summary>
    ///     Results from any interceptor operation that gives a flag to tell the event manager to proceed to the next priority
    ///     in line or not.
    /// </summary>
    public class InterceptorResult
    {
        internal InterceptorResult()
        {
        }

        /// <summary>
        ///     A boolean flag for the event manager to decide if it will continue to process or error
        /// </summary>
        public bool ContinueExecution { get; set; }

        /// <summary>
        ///     Error message populated in the case of failure
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Creates a failure result that gives the event manager a message to log, and specifies to proceed to the next
        ///     priority in line or not. The Default is False
        /// </summary>
        /// <param name="message">Error message from the interceptor</param>
        /// <param name="continueToExecute">Does execution of the interceptors continue ( Default is False )</param>
        /// <returns>An Interceptor Result</returns>
        public static InterceptorResult Failed(string message, bool continueToExecute = false)
        {
            return new InterceptorResult
            {
                ContinueExecution = continueToExecute,
                Message = message
            };
        }

        /// <summary>
        ///     Creates a successful result that continues execution
        /// </summary>
        /// <returns>An Interceptor Result</returns>
        public static InterceptorResult Succeeded()
        {
            return new InterceptorResult
            {
                ContinueExecution = true
            };
        }
    }
}
