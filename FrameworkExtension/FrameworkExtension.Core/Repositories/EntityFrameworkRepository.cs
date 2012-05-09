using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.Repositories
{
    public class EntityFrameworkRepository : IRepository
    {
        public EntityFrameworkRepository(IDataContext context)
        {
            Context = context;
        }

        public static IRepository Create<T>() where T : IDataContext
        {
            return Create<T>(string.Empty);
        }

        public static IRepository Create<T>(string connectionString) where T : IDataContext
        {
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                var contructors = typeof(T).GetConstructors();
                foreach (var constructorInfo in contructors)
                {
                    //Not Needed Yet
                    if (constructorInfo.GetParameters().Length == 1 &&
                        constructorInfo.GetParameters()[0].ParameterType == typeof(string))
                        return new EntityFrameworkRepository((IDataContext)constructorInfo.Invoke(new[] { connectionString }));
                }
                throw new InvalidOperationException("You attempted to pass a connection string to a context that doesn't have a constructor that only accepts the connection string");
            }
            return new EntityFrameworkRepository(Activator.CreateInstance<T>());
        }

        public IDataContext Context { get; private set; }
        
        public T Get<T>(IQuery<T> query) where T : class
        {
            return query.Execute(Context).FirstOrDefault();
        }

        public T Get<T>(IScalarObject<T> query)
        {
            return query.Execute(Context);
        }

        public void Execute(ICommandObject command)
        {
            command.Execute(Context);
        }

        public IEnumerable<T> Find<T>(IQuery<T> query) where T : class
        {
            return query.Execute(Context);
        }
    }
}