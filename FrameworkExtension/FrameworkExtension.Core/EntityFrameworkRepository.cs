using System;

namespace FrameworkExtension.Core
{
    public class EntityFrameworkRepository<T> : IRepository where T : IDbContext
    {
        public EntityFrameworkRepository()
            : this(string.Empty)
        {
        }


        public EntityFrameworkRepository(string connectionString)
        {
            Context = CreateContext(connectionString);
        }

        public EntityFrameworkRepository(T context)
        {
            Context = context;
        }

        private IDbContext CreateContext(string connectionString)
        {
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                var contructors = typeof(T).GetConstructors();
                foreach (var constructorInfo in contructors)
                {
                    //Not Needed Yet
                    if (constructorInfo.GetParameters().Length == 1 &&
                        constructorInfo.GetParameters()[0].ParameterType == typeof(string))
                        return (IDbContext)constructorInfo.Invoke(new[] { connectionString });
                }
            }
            return Activator.CreateInstance<T>();
        }

        public IDbContext Context { get; set; }
    }
}