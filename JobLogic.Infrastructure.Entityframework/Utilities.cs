using JobLogic.Infrastructure.Entityframework.TenancyInterceptor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Entityframework
{
    public static class Utilities
    {
        public static int ProcessSaveChanges<T>(this T tenancyDbContext, Func<int> actualSave) where T:DbContext, ITenancyDbContext
        {
            try
            {
                tenancyDbContext.IncludedDeletedEntities = true;
                SetTheUpdatedAtValueForAddedAndModifiedEntities(tenancyDbContext.ChangeTracker);

                var returnValue = actualSave();

                if (returnValue > 0)
                {
                    //We must detached all soft deleted entities after save change successfully
                    //Otherwise, don't have any changes (marked as soft deleted) in the list that entity framework need to save into database
                    DetachSoftDeletedEntitiesOutOfEntityFrameworkLocalStorage(tenancyDbContext.ChangeTracker);
                }

                tenancyDbContext.IncludedDeletedEntities = false;
                return returnValue;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new Exception(sb.ToString());
            }
            catch
            {
                throw;
            }
        }

        public static async Task<int> ProcessSaveChangesAsync<T>(this T tenancyDbContext, Func<Task<int>> actualSaveAsync) where T : DbContext, ITenancyDbContext
        {
            try
            {
                tenancyDbContext.IncludedDeletedEntities = true;
                SetTheUpdatedAtValueForAddedAndModifiedEntities(tenancyDbContext.ChangeTracker);

                var returnValue = await actualSaveAsync();

                if (returnValue > 0)
                {
                    //We must detached all soft deleted entities after save change successfully
                    //Otherwise, don't have any changes (marked as soft deleted) in the list that entity framework need to save into database
                    DetachSoftDeletedEntitiesOutOfEntityFrameworkLocalStorage(tenancyDbContext.ChangeTracker);
                }

                tenancyDbContext.IncludedDeletedEntities = false;
                return returnValue;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new Exception(sb.ToString());
            }
            catch
            {
                throw;
            }
        }

        private static void SetTheUpdatedAtValueForAddedAndModifiedEntities(DbChangeTracker changeTracker)
        {
            var updatedEnities = changeTracker.Entries<BaseEntity>()
                .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added).ToList();

            if (updatedEnities != null)
            {
                var minDate = new DateTime(1900, 1, 1);
                foreach (var item in updatedEnities)
                {
                    var utcNow = DateTimeOffset.UtcNow;

                    item.Entity.UpdatedAt = utcNow;

                    if (item.State == EntityState.Added && item.Entity.CreatedAt <= minDate)
                    {
                        item.Entity.CreatedAt = utcNow;
                    }
                }
            }
        }

        private static void DetachSoftDeletedEntitiesOutOfEntityFrameworkLocalStorage(DbChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                // If this entity is marked as soft deleted, remove it out the local storage of entityframework
                if (entry.Entity is BaseEntity baseEntity && baseEntity.IsDeleted)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
    }
}
