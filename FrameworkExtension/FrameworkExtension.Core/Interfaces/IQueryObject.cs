using System.Collections.Generic;

namespace FrameworkExtension.Core.Interfaces
{
    public interface ICommandObject
    {
        void Execute(IDataContext context);
    }

    public interface IScalarObject<out T> where T : struct
    {
        T Execute(IDataContext context);
    }

    public interface IQueryObject<out T>
    {
        IEnumerable<T> Execute(IDataContext context);
    }
}
