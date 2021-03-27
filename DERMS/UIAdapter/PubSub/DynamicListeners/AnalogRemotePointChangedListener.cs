using Common.PubSub;
using Common.PubSub.Messages;

namespace UIAdapter.PubSub.DynamicListeners
{
    public class AnalogRemotePointChangedListener : BaseMessageListener<AnalogRemotePointValueChanged>
    {
        public AnalogRemotePointChangedListener() : base(Common.PubSub.Subscriptions.Topic.AnalogRemotePointChange)
        {

        }
    }
}
