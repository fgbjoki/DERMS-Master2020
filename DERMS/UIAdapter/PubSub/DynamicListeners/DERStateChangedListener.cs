using Common.PubSub;
using Common.PubSub.Messages;
using Common.PubSub.Subscriptions;

namespace UIAdapter.PubSub.DynamicListeners
{
    public class DERStateChangedListener : BaseMessageListener<DERStateChanged>
    {
        public DERStateChangedListener() : base(Topic.DERStateChange)
        {
        }
    }
}
