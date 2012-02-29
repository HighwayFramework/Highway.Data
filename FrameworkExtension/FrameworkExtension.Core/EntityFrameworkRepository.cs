namespace FrameworkExtension.Core
{
    public class EntityFrameworkRepository<IDbContext> : IRepository
    {
        public EntityFrameworkRepository()
        {
            
        }

        public IDbContext Context { get; set; }
    }
}