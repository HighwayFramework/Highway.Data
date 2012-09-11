using System.Data.Entity;

namespace Highway.Data
{
    /// <summary>
    /// Default Settings of Lazy loading and proxy generation off.
    /// </summary>
    public class DefaultContextConfiguration : IContextConfiguration
    {
        #region IContextConfiguration Members

        public void ConfigureContext(DbContext context)
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
        }

        #endregion
    }
}