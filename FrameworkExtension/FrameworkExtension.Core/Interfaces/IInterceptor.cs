using System;
using FrameworkExtension.Core.Interceptors;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IInterceptor<in T> where T : System.EventArgs
    {
        InterceptorResult Execute(IDataContext context, T eventArgs);
        int Priority { get; set; }
    }
}
