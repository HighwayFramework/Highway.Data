using System.Data.Entity;

namespace Highway.Data
{
    /// <summary>
    ///     Configuration for the context that takes the Highway.Data opinions.
    /// </summary>
    public class DefaultContextConfiguration : IContextConfiguration
    {
        /// <summary>
        ///     Configures context without lazy loading or proxy creation
        /// </summary>
        /// <param name="context"></param>
        public void ConfigureContext(DbContext context)
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
        }
    }
}
