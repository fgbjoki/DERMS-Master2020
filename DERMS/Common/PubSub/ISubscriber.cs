using Common.PubSub.Subscriptions;
using System.Collections.Generic;

namespace Common.PubSub
{
    public interface ISubscriber
    {
        IEnumerable<ISubscription> GetSubscriptions();
    }
}
