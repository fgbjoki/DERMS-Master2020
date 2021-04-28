using Common.PubSub;
using Common.PubSub.Messages;
using Common.PubSub.Subscriptions;

namespace UIAdapter.PubSub.DynamicListeners
{
    public class EnergyBalanceChangedListener : BaseMessageListener<EnergyBalanceChanged>
    {
        public EnergyBalanceChangedListener() : base(Topic.EnergyBalanceChange)
        {
        }
    }
}
