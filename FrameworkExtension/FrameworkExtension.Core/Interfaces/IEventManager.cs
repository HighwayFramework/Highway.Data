using System;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IEventManager
    {
        void Register<T>(IInterceptor<T> interceptor) where T : System.EventArgs;
    }
}
