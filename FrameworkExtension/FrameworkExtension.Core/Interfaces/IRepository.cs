using System.Collections.Generic;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IRepository
    {
        IDataContext Context { get; }
        IEventManager EventManager { get; }
        IEnumerable<T> Find<T>(IQuery<T> query) where T : class;
        T Get<T>(IScalarObject<T> query);
        void Execute(ICommandObject command);
    }
}