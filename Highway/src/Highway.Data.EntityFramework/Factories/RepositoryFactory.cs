#region

using System;
using Highway.Data.Domain;

#endregion

namespace Highway.Data.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IContextFactory _contextFactory;

        public RepositoryFactory(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public IRepository Create<T>() where T : class, IDomain
        {
            return new Repository(_contextFactory.Create<T>());
        }

        public IRepository Create(Type type)
        {
            return new Repository(_contextFactory.Create(type));
        }
    }

    public interface IRepositoryFactory
    {
        IRepository Create<T>() where T : class, IDomain;
        IRepository Create(Type type);
    }
}