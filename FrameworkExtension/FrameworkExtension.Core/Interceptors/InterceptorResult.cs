namespace FrameworkExtension.Core.Interceptors
{
    public struct InterceptorResult
    {
        public static InterceptorResult Succeeded()
        {
            return new InterceptorResult()
                       {
                           ContinueExecution = true
                       };
        }

        public static InterceptorResult Failed(string message, bool continueToExecute = false)
        {
            return new InterceptorResult()
                       {
                           ContinueExecution = continueToExecute,
                           Message = message
                       };
        }

        public bool ContinueExecution { get; set; }
        public string Message { get; set; }
    }
}