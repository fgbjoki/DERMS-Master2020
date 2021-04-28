using CalculationEngine.CommonComponents;
using Common.AbstractModel;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class CalculatingUnitAnalogValueChanged : BaseDynamicHandler<AnalogRemotePointValueChanged>
    {
        private ITopologyDependentComponent topologyDependentComponent;

        public CalculatingUnitAnalogValueChanged(ITopologyDependentComponent topologyDependentComponent)
        {
            this.topologyDependentComponent = topologyDependentComponent;
        }

        protected override void ProcessChanges(AnalogRemotePointValueChanged message)
        {
            Property currentValueProperty = message.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Cannot find value property for analog measurement with gid: {message.Id:X16}. Skipping further processing!");
                return;
            }

            float value = currentValueProperty.AsFloat();

            topologyDependentComponent.ProcessAnalogChanges(message.Id, value);
        }
    }
}
