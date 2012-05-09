using System.Collections.Generic;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core
{
    public interface IRepository
    {
        IDataContext Context { get; }
        IEnumerable<T> Find<T>(IQuery<T> query) where T : class;
        T Get<T>(IQuery<T> query) where T : class;
        T Get<T>(IScalarObject<T> query);
        void Execute(ICommandObject command);
    }
}