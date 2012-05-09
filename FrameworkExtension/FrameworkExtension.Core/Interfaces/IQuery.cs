using System.Collections.Generic;

namespace FrameworkExtension.Core.Interfaces
{
    public interface ICommandObject
    {
        void Execute(IDataContext context);
    }

    public interface IScalarObject<out T>
    {
        T Execute(IDataContext context);
    }

    public interface IQuery<out T>
    {
        IEnumerable<T> Execute(IDataContext context);
    }
}
