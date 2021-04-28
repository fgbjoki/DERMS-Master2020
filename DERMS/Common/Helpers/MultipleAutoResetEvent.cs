using System.Threading;

namespace Common.Helpers
{
    public class AdvancedSemaphore
    {
        private SemaphoreSlim semaphore;
        private long currentWaiters;
        private int maxWaiters;

        public AdvancedSemaphore(int maxWaiters)
        {
            this.maxWaiters = maxWaiters;

            currentWaiters = 0;
            semaphore = new SemaphoreSlim(maxWaiters);
        }

        public void WaitOne()
        {
            Interlocked.Increment(ref currentWaiters);

            semaphore.Wait();

            Interlocked.Decrement(ref currentWaiters);
        } 
        
        public void Release()
        {
            long waiters = Interlocked.Read(ref currentWaiters);

            int releaseCount = -(int)waiters;
            Interlocked.Add(ref releaseCount, maxWaiters);

            if (releaseCount == 0)
            {
                return;
            }

            semaphore.Release(releaseCount);
        }
    }
}
