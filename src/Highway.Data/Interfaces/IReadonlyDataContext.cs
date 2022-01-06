using System;

namespace Highway.Data
{
    /// <summary>
    ///     Contract for a readonly Data Context
    /// </summary>
    public interface IReadonlyDataContext : IDisposable, IDataContextBase
    {
    }
}
