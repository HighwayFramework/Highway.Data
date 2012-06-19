using System;
using System.Collections.Generic;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.EventManagement
{
    public class EventManager : IEventManager
    {
        private Dictionary<object, IEnumerable<IInterceptor<System.EventArgs>>> listeners = new Dictionary<object, IEnumerable<IInterceptor<System.EventArgs>>>();

        public void Register<T>(IInterceptor<T> interceptor) where T : System.EventArgs
        {
            Type key = typeof (T);
            if(listeners.ContainsKey(key))
            {
                
            }
        }
    }
}