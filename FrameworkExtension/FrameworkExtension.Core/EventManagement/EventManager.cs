using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Interceptors.Events;

namespace FrameworkExtension.Core.EventManagement
{
    public class EventManager : IEventManager
    {
        private IObservableDataContext _context;
        private readonly List<IInterceptor<PreSaveEventArgs>> _preSaveInterceptors = new List<IInterceptor<PreSaveEventArgs>>();
        private readonly List<IInterceptor<PostSaveEventArgs>> _postSaveInterceptors = new List<IInterceptor<PostSaveEventArgs>>();

        public void Register<T>(IInterceptor<T> interceptor) where T : EventArgs
        {
            Type key = typeof(T);
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

        internal IObservableDataContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                if (Object.ReferenceEquals(_context, value))
                    return;
                _context = value;
                _context.PreSave += OnPreSave;
                _context.PostSave += OnPostSave;
            }
        }
        private void OnPreSave(object sender, PreSaveEventArgs e)
        {
            foreach (var preSaveInterceptor in _preSaveInterceptors.OrderBy(x=>x.Priority))
            {
                var result = preSaveInterceptor.Execute(Context,e);
                if (result.ContinueExecution == false) break;
            }
        }
        private void OnPostSave(object sender, PostSaveEventArgs e)
        {
            foreach (var postSaveInterceptor in _postSaveInterceptors.OrderBy(x => x.Priority))
            {
                var result = postSaveInterceptor.Execute(Context, e);
                if (result.ContinueExecution == false) break;
            }
        }
    }
}