using System.Threading;

namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithReadonlyChild
{
    public class Parent : IIdentifiable<long>
    {
        private static Child _child;

        private readonly ReaderWriterLockSlim _lock = new();

        public Child Child
        {
            get
            {
                _lock.EnterUpgradeableReadLock();
                try
                {
                    _child ??= BuildChild();
                }
                finally
                {
                    _lock.ExitUpgradeableReadLock();
                }

                return _child;
            }
        }

        public long Id { get; set; }

        public string Name { get; set; }

        private Child BuildChild()
        {
            _lock.EnterWriteLock();
            try
            {
                return new Child { Name = nameof(Child) };
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
