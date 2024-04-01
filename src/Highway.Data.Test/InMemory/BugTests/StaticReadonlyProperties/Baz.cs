using System.Collections.Generic;
using System.Threading;

namespace Highway.Data.Test.InMemory.BugTests.StaticReadonlyProperties
{
    public class Baz : IIdentifiable<long>
    {
        private static readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.NoRecursion);

        private static ICollection<Qux> _bazQuxes;

        public long Id { get; set; }

        /// <summary>
        ///     Gets the security configuration.
        /// </summary>
        public ICollection<Qux> BazQuxes
        {
            get
            {
                Lock.EnterUpgradeableReadLock();

                try
                {
                    return _bazQuxes ??= BuildBazQuxes();
                }
                finally
                {
                    Lock.ExitUpgradeableReadLock();
                }
            }
        }

        public string Name { get; set; }

        private ICollection<Qux> BuildBazQuxes()
        {
            Lock.EnterWriteLock();
            try
            {
                return new List<Qux>
                {
                    new() { Id = 2, Name = $"{nameof(Qux)}2" },
                    new() { Id = 3, Name = $"{nameof(Qux)}3" }
                };
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
    }
}
