using CalculationEngine.EnergyCalculators;
using Common.AbstractModel;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class EnergyBalanceAnalogValueChanged : BaseDynamicHandler<AnalogRemotePointValueChanged>
    {
        private IEnergyBalanceCalculator energyBalanceCalculator;

        public EnergyBalanceAnalogValueChanged(IEnergyBalanceCalculator energyBalanceCalculator)
        {
            this.energyBalanceCalculator = energyBalanceCalculator;
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

            energyBalanceCalculator.Recalculate(message.Id, value);
        }
    }
}
