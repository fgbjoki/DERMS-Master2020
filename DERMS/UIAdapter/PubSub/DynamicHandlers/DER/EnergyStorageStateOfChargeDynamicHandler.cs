using Common.AbstractModel;
using Common.GDA;
using Common.PubSub;
using Common.PubSub.Messages;
using Common.TransactionProcessing.Storage.Helpers;

namespace UIAdapter.PubSub.DynamicHandlers.DER
{
    public class EnergyStorageStateOfChargeDynamicHandler : BaseDynamicHandler<AnalogRemotePointValuesChanged>
    {
        private IAnalogEntityStorage entityStorage;

        public EnergyStorageStateOfChargeDynamicHandler(IAnalogEntityStorage entityStorage)
        {
            this.entityStorage = entityStorage;
        }

        protected override void ProcessChanges(AnalogRemotePointValuesChanged message)
        {
            foreach (var analogChange in message.Changes)
            {
                Property currentValueProperty = analogChange.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE);

                if (currentValueProperty == null)
                {
                    return;
                }

                float currentValue = currentValueProperty.AsFloat();

                entityStorage.UpdateAnalogValue(analogChange.Id, currentValue);
            }           
        }
    }
}
