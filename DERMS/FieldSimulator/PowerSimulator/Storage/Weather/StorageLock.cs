using System;
using System.Threading;

namespace FieldSimulator.PowerSimulator.Storage.Weather
{
    public class StorageLock : IDisposable
    {
        private ReaderWriterLockSlim locker;

        public StorageLock(ReaderWriterLockSlim locker)
        {
            this.locker = locker;
        }

        public void EnterWriteLock()
        {
            locker.EnterWriteLock();
        }

        public void ExitWriteLock()
        {
            locker.ExitWriteLock();
        }

        public void EnterReadLock()
        {
            locker.EnterReadLock();
        }

        public void ExitReadLock()
        {
            locker.ExitReadLock();
        }

        public void Dispose()
        {
            locker.ExitReadLock();
        }
    }
}
