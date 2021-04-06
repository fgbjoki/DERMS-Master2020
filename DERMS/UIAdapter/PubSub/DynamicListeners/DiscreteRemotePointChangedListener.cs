using Common.PubSub;
using Common.PubSub.Messages;

namespace UIAdapter.PubSub.DynamicListeners
{
    public class DiscreteRemotePointChangedListener : BaseMessageListener<DiscreteRemotePointValueChanged>
    {
        public DiscreteRemotePointChangedListener() : base(Common.PubSub.Subscriptions.Topic.DiscreteRemotePointChange)
        {

        }
    }
}
