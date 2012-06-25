using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.Interceptors.Events;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IObservableDataContext : IDataContext
    {
        event EventHandler<PreSaveEventArgs> PreSave;
        event EventHandler<PostSaveEventArgs> PostSave;
    }
}
