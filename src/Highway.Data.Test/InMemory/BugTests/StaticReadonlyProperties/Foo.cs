using System.Threading;

namespace Highway.Data.Test.InMemory.BugTests.StaticReadonlyProperties
{
    public class Foo : IIdentifiable<long>
    {
        private static readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.NoRecursion);

        private static Bar _fooBar;

        /// <summary>
        ///     Gets the security configuration.
        /// </summary>
        public Bar FooBar
        {
            get
            {
                Lock.EnterUpgradeableReadLock();

                try
                {
                    return _fooBar ??= BuildFooBar();
                }
                finally
                {
                    Lock.ExitUpgradeableReadLock();
                }
            }
        }

        public long Id { get; set; }

        public string Name { get; set; }

        private Bar BuildFooBar()
        {
            Lock.EnterWriteLock();
            try
            {
                return new Bar { Id = 2, Name = $"{nameof(Bar)}2" };
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
    }
}
