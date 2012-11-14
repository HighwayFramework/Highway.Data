using System;
using Highway.Data.Interceptors.Events;
using Highway.Data.Interfaces;

namespace Highway.Data.Interceptors
{
    /// <summary>
    /// An interceptor that operates pre-save to add audit information to the records being committed that implement the <see cref="Highway.Data.Interfaces.IAuditableEntity"/> interface
    /// </summary>
    public class AuditableInterceptor : IInterceptor<PreSaveEventArgs>
    {
        private readonly IUserNameService _userNameService;

        /// <summary>
        /// Creates a interceptor for audit data attachment
        /// </summary>
        /// <param name="userNameService">Application Service that provides current user name</param>
        /// <param name="priority">The order in the priority stack that the interceptor should operate on</param>
        public AuditableInterceptor(IUserNameService userNameService, int priority = 0)
        {
            _userNameService = userNameService;
        }

        #region IInterceptor<PreSaveEventArgs> Members

        /// <summary>
        /// Executes the interceptor handle an event based on the event arguments
        /// </summary>
        /// <param name="context">The data context that raised the event</param>
        /// <param name="eventArgs">The event arguments that were passed from the context</param>
        /// <returns>An Interceptor Result</returns>
        public InterceptorResult Execute(IDataContext context, PreSaveEventArgs eventArgs)
        {
            return new InterceptorResult();
        }

        /// <summary>
        ///  The priority order that this interceptor has for ordered execution by the event manager
        /// </summary>
        public int Priority { get; set; }

        #endregion
    }
}