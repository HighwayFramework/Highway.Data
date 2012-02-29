using System.Linq;
namespace FrameworkExtension.Core
{
    public interface IDbContext
    {
        IQueryable<T> AsQueryable<T>();
    }
}