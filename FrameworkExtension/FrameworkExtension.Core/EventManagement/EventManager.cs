using System;
using System.Collections.Generic;
using FrameworkExtension.Core.Interfaces;
using FrameworkExtension.Core.Interceptors.Events;

namespace FrameworkExtension.Core.EventManagement
{
    public class EventManager : IEventManager
    {
        private IObservableDataContext _Context;
        private readonly List<IInterceptor<PreSaveEventArgs>> _PreSaveInterceptors = new List<IInterceptor<PreSaveEventArgs>>();
        private readonly List<IInterceptor<PostSaveEventArgs>> _PostSaveInterceptors = new List<IInterceptor<PostSaveEventArgs>>();

        public void Register<T>(IInterceptor<T> interceptor) where T : EventArgs
        {
            Type key = typeof(T);
            switch (key.Name)
            {
                case "PreSaveEventArgs":
                    _PreSaveInterceptors.Add(interceptor as IInterceptor<PreSaveEventArgs>);
                    break;
                case "PostSaveEventArgs":
                    _PostSaveInterceptors.Add(interceptor as IInterceptor<PostSaveEventArgs>);
                    break;
                default:
                    throw new ArgumentException("Only PreSaveEventArgs and PostSaveEventArgs");
            }
        }

        internal IObservableDataContext Context
        {
            get
            {
                return _Context;
            }
            set
            {
                if (Object.ReferenceEquals(_Context, value))
                    return;
                _Context = value;
                _Context.PreSave += OnPreSave;
                _Context.PostSave += OnPostSave;
            }
        }
        private void OnPreSave(object sender, PreSaveEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void OnPostSave(object sender, PostSaveEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}