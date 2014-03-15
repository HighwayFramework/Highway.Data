using System;
using Highway.Data.Contexts;
using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    /// <summary>
    /// In memory test stub for context configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryDomainContext<T> : InMemoryDataContext, IDomainContext<T> where T : class, IDomain
    {
        public override int Commit()
        {
            OnBeforeSave(new BeforeSave());
            base.Commit();
            OnAfterSave(new AfterSave());
            return 1;
        }

        /// <summary>
        /// event before save
        /// </summary>
        public event EventHandler<BeforeSave> BeforeSave;

        protected virtual void OnBeforeSave(BeforeSave e)
        {
            EventHandler<BeforeSave> handler = BeforeSave;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// event after save
        /// </summary>
        public event EventHandler<AfterSave> AfterSave;

        protected virtual void OnAfterSave(AfterSave e)
        {
            EventHandler<AfterSave> handler = AfterSave;
            if (handler != null) handler(this, e);
        }
    }
}