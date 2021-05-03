using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.Model;

namespace UIAdapter.PubSub.DynamicHandlers
{
    public class AnalogRemotePointStorageDynamicHandler : BaseDynamicHandler<AnalogRemotePointValuesChanged>
    {
        private IStorage<AnalogRemotePoint> storage;

        public AnalogRemotePointStorageDynamicHandler(IStorage<AnalogRemotePoint> storage)
        {
            this.storage = storage;
        }

        protected override void ProcessChanges(AnalogRemotePointValuesChanged message)
        {
            foreach (var analogChange in message)
            {
                Property currentValueProperty = analogChange.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE);

                if (currentValueProperty == null)
                {
                    return;
                }

                float value = currentValueProperty.AsFloat();

                AnalogRemotePoint remotePoint = storage.GetEntity(analogChange.Id);

                if (remotePoint == null)
                {
                    Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {analogChange.Id:X16}. Skipping further processing!");
                }

                remotePoint.Value = value;
            }         
        }
    }
}
