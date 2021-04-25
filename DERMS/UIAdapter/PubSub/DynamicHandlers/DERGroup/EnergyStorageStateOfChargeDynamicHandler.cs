using Common.AbstractModel;
using Common.GDA;
using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.TransactionProcessing.Storages.Helpers;

namespace UIAdapter.PubSub.DynamicHandlers.DERGroup
{
    public class EnergyStorageStateOfChargeDynamicHandler : BaseDynamicHandler<AnalogRemotePointValueChanged>
    {
        private IAnalogEntityStorage entityStorage;

        public EnergyStorageStateOfChargeDynamicHandler(IAnalogEntityStorage entityStorage)
        {
            this.entityStorage = entityStorage;
        }

        protected override void ProcessChanges(AnalogRemotePointValueChanged message)
        {
            Property currentValueProperty = message.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return;
            }

            float currentValue = currentValueProperty.AsFloat();

            entityStorage.UpdateAnalogValue(message.Id, currentValue);
        }
    }
}
