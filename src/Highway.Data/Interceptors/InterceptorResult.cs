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
    }
}