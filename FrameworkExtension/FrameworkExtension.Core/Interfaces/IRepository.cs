namespace FrameworkExtension.Core
{
    public interface IRepository
    {
        IDbContext Context { get; set; }
    }
}