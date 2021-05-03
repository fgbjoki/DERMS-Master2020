using Common.PubSub;
using Common.PubSub.Messages;

namespace UIAdapter.PubSub.DynamicListeners
{
    public class DiscreteRemotePointChangedListener : BaseMessageListener<DiscreteRemotePointValuesChanged>
    {
        public DiscreteRemotePointChangedListener() : base(Common.PubSub.Subscriptions.Topic.DiscreteRemotePointChange)
        {

        }
    }
}
