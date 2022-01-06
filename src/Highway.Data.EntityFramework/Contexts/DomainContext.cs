using System;
using System.Threading.Tasks;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    /// <summary>
    ///     A Context that is constrained to a specified Domain
    /// </summary>
    /// <typeparam name="T">The Domain this context is specific for</typeparam>
    public class DomainContext<T> : DataContext, IDomainContext<T>
        where T : class, IDomain
    {
        /// <summary>
        ///     Constructs the domain context
        /// </summary>
        /// <param name="domain"></param>
        public DomainContext(T domain)
            : base(domain.ConnectionString, domain.Mappings, domain.Context, new NoOpLogger())
        {
        }

        /// <summary>
        ///     Constructs the domain context
        /// </summary>
        /// <param name="domain">domain for context</param>
        /// <param name="logger">logger</param>
        public DomainContext(T domain, ILog logger)
            : base(domain.ConnectionString, domain.Mappings, domain.Context, logger)
        {
        }

        /// <summary>
        ///     The event fired just before the commit of the ORM
        /// </summary>
        public new event EventHandler<BeforeSave> BeforeSave;

        /// <summary>
        ///     The event fired just after the commit of the ORM
        /// </summary>
        public new event EventHandler<AfterSave> AfterSave;

        public override int Commit()
        {
            OnBeforeSave();
            var changes = base.Commit();
            OnAfterSave();

            return changes;
        }

        public override async Task<int> CommitAsync()
        {
            OnBeforeSave();
            var changes = await base.CommitAsync();
            OnAfterSave();

            return changes;
        }

        private void OnAfterSave()
        {
            AfterSave?.Invoke(this, new AfterSave());
        }

        private void OnBeforeSave()
        {
            BeforeSave?.Invoke(this, new BeforeSave());
        }
    }
}
