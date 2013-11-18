#region

using System;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EventManagement;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors.Events;

#endregion

namespace Highway.Data.Interceptors
{
    /// <summary>
    ///     An eventInterceptor that operates pre-save to add audit information to the records being committed that implement the
    ///     <see cref="Highway.Data.IAuditableEntity" /> interface
    /// </summary>
    public class AuditableEventInterceptor : IEventInterceptor<BeforeSave>
    {
        private readonly IUserNameService _userNameService;

        /// <summary>
        ///     Creates a eventInterceptor for audit data attachment
        /// </summary>
        /// <param name="userNameService">Application Service that provides current user name</param>
        /// <param name="priority">The order in the priority stack that the eventInterceptor should operate on</param>
        public AuditableEventInterceptor(IUserNameService userNameService, int priority = 0)
        {
            _userNameService = userNameService;
        }

        #region IEventInterceptor<PreSaveEventArgs> Members

        /// <summary>
        ///     The priority order that this eventInterceptor has for ordered execution by the event manager
        /// </summary>
        public int Priority { get; set; }

        public InterceptorResult Apply(IDataContext dataContext, BeforeSave eventArgs)
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

        #endregion
    }
}