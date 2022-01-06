using System;
using System.Linq;

using Highway.Data.Repositories;

namespace Highway.Data.Factories
{
    /// <summary>
    ///     Simple factory for constructing repositories
    /// </summary>
    public class DomainRepositoryFactory : IDomainRepositoryFactory, IReadonlyDomainRepositoryFactory
    {
        private readonly IDomain[] _domains;

        /// <summary>
        ///     Creates a repository factory for the supplied list of domains
        /// </summary>
        /// <param name="domains">Domains to support construction for</param>
        public DomainRepositoryFactory(IDomain[] domains)
        {
            _domains = domains;
        }

        /// <summary>
        ///     Creates a repository for the requested domain
        /// </summary>
        /// <typeparam name="T">Domain for repository</typeparam>
        /// <returns>Domain specific repository</returns>
        public IRepository Create<T>()
            where T : class, IDomain
        {
            var domain = _domains.OfType<T>().SingleOrDefault();
            var context = new DomainContext<T>(domain);

            return new DomainRepository<T>(context, domain);
        }

        /// <summary>
        ///     Creates a repository for the requested domain
        /// </summary>
        /// <param name="type">Domain for repository</param>
        /// <returns>Domain specific repository</returns>
        public IRepository Create(Type type)
        {
            return (IRepository)CreateRepository(type, typeof(DomainContext<>), typeof(DomainRepository<>));
        }

        /// <summary>
        ///     Creates a readonly repository for the requested domain
        /// </summary>
        /// <typeparam name="T">Domain for repository</typeparam>
        /// <returns>Domain-specific readonly repository</returns>
        public IReadonlyRepository CreateReadonly<T>()
            where T : class, IDomain
        {
            var domain = _domains.OfType<T>().SingleOrDefault();
            var context = new ReadonlyDomainContext<T>(domain);

            return new ReadonlyDomainRepository<T>(context, domain);
        }

        /// <summary>
        ///     Creates a readonly repository for the requested domain
        /// </summary>
        /// <param name="type">Domain for repository</param>
        /// <returns>Domain-specific readonly repository</returns>
        public IReadonlyRepository CreateReadonly<T>(Type type)
        {
            return (IReadonlyRepository)CreateRepository(type, typeof(ReadonlyDomainContext<>), typeof(ReadonlyDomainRepository<>));
        }

        private object CreateRepository(Type domainType, Type contextType, Type repositoryType)
        {
            Type[] typeArgs = { domainType };

            var domain = _domains.SingleOrDefault(x => x.GetType() == domainType);
            var contextCtor = contextType.MakeGenericType(typeArgs);
            var untypedObject = Activator.CreateInstance(contextCtor, domain);

            var repositoryCtor = repositoryType.MakeGenericType(typeArgs);
            var repo = Activator.CreateInstance(repositoryCtor, untypedObject, domain);

            return (IRepository)repo;
        }
    }
}
