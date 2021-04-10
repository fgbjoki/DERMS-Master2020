using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicListeners
{
    public class AnalogRemotePointChangedListener : BaseMessageListener<AnalogRemotePointValueChanged>
    {
        public AnalogRemotePointChangedListener() : base(Common.PubSub.Subscriptions.Topic.AnalogRemotePointChange)
        {

        }
    }
}
