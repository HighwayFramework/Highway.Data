<<<<<<< HEAD
﻿using System;
using Highway.Data.Domain;
=======
﻿#region

using System;
using System.Linq;
using Highway.Data.Repositories;
>>>>>>> WIP

namespace Highway.Data.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IDomain[] _domains;

        public RepositoryFactory(IDomain[] domains)
        {
            _domains = domains;
        }


        public IRepository Create<T>() where T : class, IDomain
        {
            var domain = _domains.OfType<T>().SingleOrDefault();
            var context = new DomainContext<T>(domain);
            return new DomainRepository<T>(context,domain);
        }

        public IRepository Create(Type type)
        {
            var domain = _domains.FirstOrDefault(x => x.GetType() == type);
            var d1 = typeof(DomainContext<>);
            Type[] typeArgs = { type };
            var contextCtor = d1.MakeGenericType(typeArgs);
            object untypedObject = Activator.CreateInstance(contextCtor, domain);

            var r1 = typeof(DomainRepository<>);
            var repositoryCtor = r1.MakeGenericType(typeArgs);
            object repo = Activator.CreateInstance(repositoryCtor, untypedObject, domain);
            return (IRepository) repo;
        }
<<<<<<< HEAD
    }
=======
    }
>>>>>>> WIP
}