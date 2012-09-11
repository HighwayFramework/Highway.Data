using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;
using Highway.Data.Interfaces;

namespace Highway.Data.EventManagement
{
    /// <summary>
    /// The base implementation of the Event manager for registration of Interceptors, and execution of them in an ordered fashion
    /// </summary>
    public class EventManager : IEventManager
    {
        private readonly List<IInterceptor<PostSaveEventArgs>> _postSaveInterceptors =
            new List<IInterceptor<PostSaveEventArgs>>();

        private readonly List<IInterceptor<PreSaveEventArgs>> _preSaveInterceptors =
            new List<IInterceptor<PreSaveEventArgs>>();

        private IObservableDataContext _context;

        #region IEventManager Members

        /// <summary>
        /// Allows for the Registration of <see cref="IInterceptor{T}"/> objects that will hook to events in priority order
        /// </summary>
        /// <param name="interceptor">The interceptor to be registered to an event</param>
        /// <typeparam name="T">The Event Args that the interceptor accepts</typeparam>
        public void Register<T>(IInterceptor<T> interceptor) where T : EventArgs
        {
            Type key = typeof (T);
            switch (key.Name)
            {
                case "PreSaveEventArgs":
                    _preSaveInterceptors.Add(interceptor as IInterceptor<PreSaveEventArgs>);
                    break;
                case "PostSaveEventArgs":
                    _postSaveInterceptors.Add(interceptor as IInterceptor<PostSaveEventArgs>);
                    break;
                default:
                    throw new ArgumentException("Only PreSaveEventArgs and PostSaveEventArgs");
            }
        }

        /// <summary>
        /// A reference to context that allows for usaged and event wiring
        /// </summary>
        public IObservableDataContext Context
        {
            get { return _context; }
            set
            {
                if (ReferenceEquals(_context, value))
                    return;
                _context = value;
                _context.PreSave += OnPreSave;
                _context.PostSave += OnPostSave;
            }
        }

        #endregion

        private void OnPreSave(object sender, PreSaveEventArgs e)
        {
            foreach (var preSaveInterceptor in _preSaveInterceptors.OrderBy(x => x.Priority))
            {
                InterceptorResult result = preSaveInterceptor.Execute(Context, e);
                if (result.ContinueExecution == false) break;
            }
        }

        private void OnPostSave(object sender, PostSaveEventArgs e)
        {
            foreach (var postSaveInterceptor in _postSaveInterceptors.OrderBy(x => x.Priority))
            {
                InterceptorResult result = postSaveInterceptor.Execute(Context, e);
                if (result.ContinueExecution == false) break;
            }
        }
    }
}