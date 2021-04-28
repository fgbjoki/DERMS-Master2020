using CalculationEngine.CommonComponents;
using Common.PubSub;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class TopologyChangedDynamicHandler : IDynamicHandler
    {
        private ITopologyDependentComponent topologyDependentComponent;

        public TopologyChangedDynamicHandler(ITopologyDependentComponent topologyDependentComponent)
        {
            this.topologyDependentComponent = topologyDependentComponent;
        }

        public void ProcessChanges(object changes)
        {
            topologyDependentComponent.ProcessTopologyChanges();
        }
    }
}
