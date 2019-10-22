
using System;
using System.Data.Entity;


namespace Highway.Data
{
    public static class EntityExtensions
    {
        public static void AttachEntity<T>(this T entity, DbContext context) where T : class
        {
            try
            {
                context.Set<T>().Attach(entity);
            }
            catch (Exception)
            {
            }
        }
    }
}