using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Core.Common.ReliableCollectionProxy
{
    public static class ReliableQueueCollectionProxy
    {
        public static void Enqueue<T>(IReliableStateManager stateManager, string queueName, T item)
        {
            using (var tx = stateManager.CreateTransaction())
            {
                var queue = stateManager.GetOrAddAsync<IReliableConcurrentQueue<T>>(queueName).GetAwaiter().GetResult();

                queue.EnqueueAsync(tx, item).GetAwaiter().GetResult();

                tx.CommitAsync().GetAwaiter().GetResult();
            }
        }

        public static bool Dequeue<T>(IReliableStateManager stateManager, string queueName, out T dequeueudItem)
        {
            using (var tx = stateManager.CreateTransaction())
            {
                var queue = stateManager.GetOrAddAsync<IReliableConcurrentQueue<T>>(queueName).GetAwaiter().GetResult();

                var conditionVariable = queue.TryDequeueAsync(tx).GetAwaiter().GetResult();

                if (conditionVariable.HasValue)
                {
                    dequeueudItem = conditionVariable.Value;
                    tx.CommitAsync().GetAwaiter().GetResult();
                    return true;
                }
                else
                {
                    dequeueudItem = default(T);
                    return false;
                }
            }
        }
    }
}
