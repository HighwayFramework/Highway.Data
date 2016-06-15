using System;
using System.Data;

namespace Highway.Data.EntityFramework
{
    public static class DbContextExtentions
    {
        public static void ValidateAndCommit(this IUnitOfWork context)
        {
            try
            {
                context.Commit();
            }
            catch (DataException x)
            {
                string fullMessage = x.FullMessage();
                throw new InvalidOperationException(fullMessage, x);
            }
        }
    }
}
