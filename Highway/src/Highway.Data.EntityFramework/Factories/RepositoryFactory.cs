using System;
﻿using System.Linq;
﻿using Highway.Data.Repositories;

namespace Highway.Data.Factories
{
    /// <summary>
    /// Simple factory for constructing repositories
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IDomain[] _domains;

        /// <summary>
        /// Creates a repository factory for the supplied list of domains
        /// </summary>
        /// <param name="domains">Domains to support construction for</param>
        public RepositoryFactory(IDomain[] domains)
        {
            _domains = domains;
        }


        /// <summary>
        /// Creates a repository for the requested domain
        /// </summary>
        /// <typeparam name="T">Domain for repository</typeparam>
        /// <returns>Domain specific repository</returns>
        public IRepository Create<T>() where T : class, IDomain
        {
            var domain = _domains.OfType<T>().SingleOrDefault();
            var context = new DomainContext<T>(domain);
            return new DomainRepository<T>(context, domain);
        }

        /// <summary>
        /// Creates a repository for the requested domain
        /// </summary>
        /// <param name="type">Domain for repository</param>
        /// <returns>Domain specific repository</returns>
        public IRepository Create(Type type)
        {
            var domain = _domains.FirstOrDefault(x => x.GetType() == type);
            var d1 = typeof (DomainContext<>);
            Type[] typeArgs = {type};
            var contextCtor = d1.MakeGenericType(typeArgs);
            object untypedObject = Activator.CreateInstance(contextCtor, domain);

            var r1 = typeof (DomainRepository<>);
            var repositoryCtor = r1.MakeGenericType(typeArgs);
            object repo = Activator.CreateInstance(repositoryCtor, untypedObject, domain);
            return (IRepository) repo;
        }
    }
}