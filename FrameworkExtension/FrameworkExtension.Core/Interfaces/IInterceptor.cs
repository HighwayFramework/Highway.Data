using System;
using FrameworkExtension.Core.Interceptors;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IInterceptor<in T> where T : System.EventArgs
    {
        InterceptorResult Execute(IRepository context, T eventArgs);
    }
}
