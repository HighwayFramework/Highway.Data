#region

using System;
using Common.Logging.Simple;
using Highway.Data.Domain;
using Highway.Data.Interceptors.Events;

#endregion

namespace Highway.Data
{
    /// <summary>
    /// Context that allows 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DomainContext<T> : DataContext, IDomainContext<T> where T : class, IDomain
    {
        public DomainContext(T domain)
            : base(domain.ConnectionString, domain.Mappings, domain.Context, new NoOpLogger())
        {
            foreach (var @event in domain.Events)
            {
                EventManager.Register(@event);
            }
        }


        /// <summary>
        ///     The event fired just before the commit of the ORM
        /// </summary>
        public event EventHandler<BeforeSave> BeforeSave;

        /// <summary>
        ///     The event fired just after the commit of the ORM
        /// </summary>
        public event EventHandler<AfterSave> AfterSaved;
        private void InvokePostSave()
        {
            if (AfterSaved != null) AfterSaved(this, new AfterSave());
        }

        private void InvokePreSave()
        {
            if (BeforeSave != null) BeforeSave(this, new BeforeSave());
        }
    }
}