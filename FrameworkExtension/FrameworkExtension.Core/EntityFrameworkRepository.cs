using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core
{
    public class EntityFrameworkRepository : IRepository
    {
        public EntityFrameworkRepository(IDbContext context)
        {
            Context = context;
        }

        public static IRepository Create<T>() where T : IDbContext
        {
            return Create<T>(string.Empty);
        }

        public static IRepository Create<T>(string connectionString) where T : IDbContext
        {
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                var contructors = typeof(T).GetConstructors();
                foreach (var constructorInfo in contructors)
                {
                    //Not Needed Yet
                    if (constructorInfo.GetParameters().Length == 1 &&
                        constructorInfo.GetParameters()[0].ParameterType == typeof(string))
                        return new EntityFrameworkRepository((IDbContext)constructorInfo.Invoke(new[] { connectionString }));
                }
                throw new InvalidOperationException("You attempted to pass a connection string to a context that doesn't have a constructor that only accepts the connection string");
            }
            return new EntityFrameworkRepository(Activator.CreateInstance<T>());
        }

        public IDbContext Context { get; private set; }
        IEnumerable<T> IRepository.Find<T>(IQueryObject<T> query)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(IQueryObject<T> query)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(IScalarObject<T> query) where T : struct
        {
            throw new NotImplementedException();
        }

        public void Execute(ICommandObject command)
        {
            throw new NotImplementedException();
        }

        public void Find<TType>(IQueryObject<TType> query) where TType : class
        {
            Context.AsQueryable<TType>();
        }
    }
}