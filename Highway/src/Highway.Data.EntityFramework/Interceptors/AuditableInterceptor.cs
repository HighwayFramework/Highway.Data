#region

using System;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EventManagement;
using Highway.Data.EventManagement.Interfaces;

#endregion

namespace Highway.Data.Interceptors
{
    /// <summary>
    ///     An interceptor that operates pre-save to add audit information to the records being committed that implement the
    ///     <see cref="Highway.Data.IAuditableEntity" /> interface
    /// </summary>
    public class AuditableInterceptor : IInterceptor
    {
        private readonly IUserNameService _userNameService;

        /// <summary>
        ///     Creates a interceptor for audit data attachment
        /// </summary>
        /// <param name="userNameService">Application Service that provides current user name</param>
        /// <param name="priority">The order in the priority stack that the interceptor should operate on</param>
        public AuditableInterceptor(IUserNameService userNameService, int priority = 0)
        {
            _userNameService = userNameService;
        }

        #region IInterceptor<PreSaveEventArgs> Members

        /// <summary>
        ///     The priority order that this interceptor has for ordered execution by the event manager
        /// </summary>
        public int Priority { get; set; }

        public bool AppliesTo(EventType eventType)
        {
            return eventType.HasFlag(EventType.BeforeSave);
        }

        public InterceptorResult Apply(IDataContext dataContext, EventType eventType)
        {
            var efContext = dataContext as DbContext;
            if (efContext == null)
                throw new InvalidOperationException(
                    "Entity Framework Interceptors must be used with Entity Framework Contexts");

            string userName = _userNameService.GetCurrentUserName();
#if DEBUG
            var addedEntities = efContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Where(e => e.Entity is IAuditableEntity).ToList();
            var modifiedEntities = efContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Where(e => e.Entity is IAuditableEntity).ToList();
            var deletedEntities = efContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Where(e => e.Entity is IAuditableEntity).ToList();
#endif

            efContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added)
                .Where(e => e.Entity is IAuditableEntity)
                .ToList()
                .ForEach(e =>
                {
                    var entity = e.Entity as IAuditableEntity;
                    if (entity != null)
                    {
                        entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
                        entity.CreatedBy = entity.ModifiedBy = userName;
                    }
                });

            efContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified)
                .Where(e => e.Entity is IAuditableEntity)
                .ToList()
                .ForEach(e =>
                {
                    var entity = e.Entity as IAuditableEntity;
                    if (entity != null)
                    {
                        entity.ModifiedDate = DateTime.Now;
                        entity.ModifiedBy = userName;
                    }
                });
            efContext.ChangeTracker.DetectChanges();
            return InterceptorResult.Succeeded();
        }

        /// <summary>
        ///     Executes the interceptor handle an event based on the event arguments
        /// </summary>
        /// <param name="context">The data context that raised the event</param>
        /// <param name="eventArgs">The event arguments that were passed from the context</param>
        /// <returns>An Interceptor Result</returns>
        public InterceptorResult Execute(IDataContext context, InterceptorEventArgs eventArgs)
        {
            return InterceptorResult.Succeeded();
        }

        #endregion
    }
}