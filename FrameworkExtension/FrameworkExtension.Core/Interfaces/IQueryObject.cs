using System.Collections.Generic;

namespace FrameworkExtension.Core.Interfaces
{
    public interface ICommandObject
    {
        void Execute(IDbContext context);
    }

    public interface IScalarObject<out T> where T : struct
    {
        T Execute(IDbContext context);
    }

    public interface IQueryObject<out T>
    {
        IEnumerable<T> Execute(IDbContext context);
    }
}
