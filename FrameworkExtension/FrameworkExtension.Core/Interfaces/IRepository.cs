using System.Collections.Generic;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core
{
    public interface IRepository
    {
        IDbContext Context { get; }
        IEnumerable<T> Find<T>(IQueryObject<T> query);
        T Get<T>(IQueryObject<T> query);
        T Get<T>(IScalarObject<T> query) where T : struct;
        void Execute(ICommandObject command);
    }
}