
using System.Data.Common;


namespace Highway.Data.Factories
{
    public interface IDataContextFactory
    {
        IDataContext Create();
        IDataContext Create(DbConnection connection);
    }
}