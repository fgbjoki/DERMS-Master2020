using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TopologyAnalysis;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class BreakerStateChangedTopologyAnalysisDynamicHandler : BaseDynamicHandler<DiscreteRemotePointValuesChanged>
    {
        private ITopologyModifier topologyModifier;
        private IStorage<DiscreteRemotePoint> discreteRemotePointStorage;

        public BreakerStateChangedTopologyAnalysisDynamicHandler(ITopologyModifier topologyModifier, IStorage<DiscreteRemotePoint> discreteRemotePointStorage)
        {
            this.topologyModifier = topologyModifier;
            this.discreteRemotePointStorage = discreteRemotePointStorage;
        }

        protected override void ProcessChanges(DiscreteRemotePointValuesChanged message)
        {
            foreach (var discreteChange in message.Changes)
            {
                Property currentValueProperty = discreteChange.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

                if (currentValueProperty == null)
                {
                    return;
                }

                int value = currentValueProperty.AsInt();

                DiscreteRemotePoint discreteRemotePoint = discreteRemotePointStorage.GetEntity(discreteChange.Id);

                if (discreteRemotePoint == null)
                {
                    Logger.Instance.Log($"[{GetType()}] Cannot find discrete remote point with gid: {discreteChange.Id:X16}. Topology will not be aligned with current network state!");
                    return;
                }

                topologyModifier.Write(discreteRemotePoint.BreakerGid, value);
            }
            
        }
    }
}
