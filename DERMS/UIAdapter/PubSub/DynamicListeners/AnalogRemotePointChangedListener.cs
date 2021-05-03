using Common.PubSub;
using Common.PubSub.Messages;

namespace UIAdapter.PubSub.DynamicListeners
{
    public class AnalogRemotePointChangedListener : BaseMessageListener<AnalogRemotePointValuesChanged>
    {
        public AnalogRemotePointChangedListener() : base(Common.PubSub.Subscriptions.Topic.AnalogRemotePointChange)
        {

        }
    }
}
