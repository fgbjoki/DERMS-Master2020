using System;
using System.Threading;
using System.Collections.Generic;

namespace FieldProcessor
{
    public class BlockingQueue<T> : IDisposable
    {
        private static readonly int LOCKER_TIMEOUT = 1000; // 1 second
        private AutoResetEvent semaphore;

        private object locker;

        private List<T> items;

        public BlockingQueue()
        {
            locker = new ReaderWriterLock();
            semaphore = new AutoResetEvent(false);
            items = new List<T>();
        }

        public bool Enqueue(T item)
        {
            try
            {
                lock (locker)
                {
                    items.Add(item);
                    semaphore.Set();
                }

                return true;
            }
            catch (ApplicationException ae)
            {
                // log
            }

            return false;
        }

        public T Dequeue()
        {
            T item = default(T);

            try
            {
                int collectionSize;
                lock (locker)
                {
                    collectionSize = items.Count;
                }

                if (collectionSize == 0)
                {
                    semaphore.WaitOne();
                }

                lock (locker)
                {
                    collectionSize = items.Count;
                }

                if (collectionSize == 0)
                {
                    return default(T);
                }

                lock (locker)
                {
                    item = items[0];
                    items.RemoveAt(0);
                }
            }
            catch (Exception e)
            {
                // log
            }

            return item;
        }

        public void Dispose()
        {
            lock (locker)
            {
                items.Clear();
                semaphore.Set();
            }
        }
    }
}
