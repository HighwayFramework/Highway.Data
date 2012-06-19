using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using FrameworkExtension.Core.Interceptors.Events;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.Interceptors
{
    public class EntityFrameworkAuditableInterceptor : IInterceptor<PreSaveEventArgs>
    {
        private IUserNameService _userNameService;

        public EntityFrameworkAuditableInterceptor(IUserNameService userNameService)
        {
            _userNameService = userNameService;
        }

        public InterceptorResult Execute(IRepository context, PreSaveEventArgs eventArgs)
        {
            var efContext = context as DbContext;
            if (efContext == null)
                throw new InvalidOperationException(
                    "Entity Framework Interceptors must be used with Entity Framework Contexts");
            
            var userName = _userNameService.GetCurrentUserName();
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
                    entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
                    entity.CreatedBy = entity.ModifiedBy = userName;
                });

            efContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified)
                .Where(e => e.Entity is IAuditableEntity)
                .ToList()
                .ForEach(e =>
                {
                    var entity = e.Entity as IAuditableEntity;
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = userName;
                });
            efContext.ChangeTracker.DetectChanges();
            return new InterceptorResult();
        }
    }
}