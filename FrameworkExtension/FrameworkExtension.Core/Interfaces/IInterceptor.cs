using System;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IInterceptor<T> where T : System.EventArgs
    {
        bool Execute(IDataContext context, T eventArgs);
    }
}
