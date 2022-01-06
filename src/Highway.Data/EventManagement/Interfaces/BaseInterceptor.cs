using Highway.Data.Interceptors;

namespace Highway.Data.EventManagement.Interfaces
{
    /// <summary>
    ///     Base interface for dealing with event interception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventInterceptor<in T> : IInterceptor
    {
        bool AppliesTo(T eventArgs);

        /// <summary>
        ///     Executes the interceptors that were applied during the domain creation
        /// </summary>
        /// <param name="context">The DataContext that the current interceptor is using</param>
        /// <param name="eventArgs">the EventArgs for the current event being intercepted</param>
        /// <returns></returns>
        InterceptorResult Apply(IQueryableProvider context, T eventArgs);
    }

    /// <summary>
    ///     Base interface for dealing with event interception
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        ///     Priority for the interceptor, execution is low to high
        /// </summary>
        int Priority { get; }
    }
}
