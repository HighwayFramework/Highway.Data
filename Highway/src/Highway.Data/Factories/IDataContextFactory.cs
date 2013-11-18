#region

using System.Data.Common;

#endregion

namespace Highway.Data.Factories
{
    public interface IDataContextFactory
    {
        IDataContext Create();
        IDataContext Create(DbConnection connection);
    }
}