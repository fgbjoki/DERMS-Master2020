using Microsoft.ServiceFabric.Data;
using System.Collections.Generic;
using System.Threading;

namespace Core.Common.ReliableCollectionProxy
{
    public static class AsyncEnumeratorHandler
    {
        public static IEnumerable<EntityType> Enumerate<EntityType>(IAsyncEnumerable<EntityType> asyncEnumerable)
        {
            var enumerable = asyncEnumerable.GetAsyncEnumerator();

            while (enumerable.MoveNextAsync(CancellationToken.None).GetAwaiter().GetResult())
            {
                yield return enumerable.Current;
            }
        }
    }
}
