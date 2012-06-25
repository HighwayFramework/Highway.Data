using System;
using FrameworkExtension.Core.Interceptors.Events;

namespace FrameworkExtension.Core.Interfaces
{
    internal interface IObservableQuery
    {
        event EventHandler<PreQueryEventArgs> PreQuery;
        event EventHandler<PostQueryEventArgs> PostQuery;
    }
}