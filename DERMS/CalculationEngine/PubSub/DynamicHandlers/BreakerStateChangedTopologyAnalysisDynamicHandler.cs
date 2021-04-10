using CalculationEngine.TopologyAnalysis;
using Common.AbstractModel;
using Common.GDA;
using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class BreakerStateChangedTopologyAnalysisDynamicHandler : BaseDynamicHandler<DiscreteRemotePointValueChanged>
    {
        private ITopologyModifier topologyModifier;

        public BreakerStateChangedTopologyAnalysisDynamicHandler(ITopologyModifier topologyModifier)
        {
            this.topologyModifier = topologyModifier;
        }

        protected override void ProcessChanges(DiscreteRemotePointValueChanged message)
        {
            Property currentValueProperty = message.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return;
            }

            int value = currentValueProperty.AsInt();

            topologyModifier.Write(message.Id, value);
        }
    }
}
