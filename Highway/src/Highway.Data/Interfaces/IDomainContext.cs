using System;
using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public interface IDomainContext<in T> : IDataContext where T : class
    {
        /// <summary>
        ///     The event fired just before the commit of the persistence
        /// </summary>
        event EventHandler<BeforeSave> BeforeSave;

        /// <summary>
        ///     The event fired just after the commit of the persistence
        /// </summary>
        event EventHandler<AfterSave> AfterSave;
    }
}