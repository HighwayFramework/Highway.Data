using System.Collections.Generic;
using System.Threading;

namespace Highway.Data.Test.InMemory.BugTests.ReadonlyProperties.ParentsWithReadonlyChildren
{
    public class Parent : IIdentifiable<long>
    {
        private static ICollection<Child> _children;

        private readonly ReaderWriterLockSlim _lock = new();

        public ICollection<Child> Children
        {
            get
            {
                _lock.EnterUpgradeableReadLock();
                try
                {
                    _children ??= BuildChildren();
                }
                finally
                {
                    _lock.ExitUpgradeableReadLock();
                }

                return _children;
            }
        }

        public long Id { get; set; }

        public string Name { get; set; }

        private ICollection<Child> BuildChildren()
        {
            _lock.EnterWriteLock();
            try
            {
                return new List<Child>
                {
                    new() { Name = $"{nameof(Child)}1" },
                    new() { Name = $"{nameof(Child)}2" }
                };
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
