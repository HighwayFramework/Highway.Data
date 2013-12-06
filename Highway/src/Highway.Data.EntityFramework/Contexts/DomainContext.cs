using System;
using Common.Logging.Simple;
using Highway.Data.Interceptors.Events;


namespace Highway.Data
{
    /// <summary>
    /// A Context that is constrained to a specified Domain
    /// </summary>
    /// <typeparam name="T">The Domain this context is specific for</typeparam>
    public class DomainContext<T> : DataContext, IDomainContext<T> where T : class, IDomain
    {
        /// <summary>
        /// Constructs the domain context
        /// </summary>
        /// <param name="domain"></param>
        public DomainContext(T domain)
            : base(domain.ConnectionString, domain.Mappings, domain.Context, new NoOpLogger())
        {
        }


        public override int Commit()
        {
            OnBeforeSave();
            var changes = base.Commit();
            OnAfterSave();
            return changes;
        }

        /// <summary>
        ///     The event fired just before the commit of the ORM
        /// </summary>
        public event EventHandler<BeforeSave> BeforeSave;

        /// <summary>
        ///     The event fired just after the commit of the ORM
        /// </summary>
        public event EventHandler<AfterSave> AfterSave;

        private void OnAfterSave()
        {
            if (AfterSave != null) AfterSave(this, new AfterSave());
        }

        private void OnBeforeSave()
        {
            if (BeforeSave != null) BeforeSave(this, new BeforeSave());
        }
    }
}