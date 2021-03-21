using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.Model;

namespace UIAdapter.PubSub.DynamicHandlers
{
    public class AnalogRemotePointStorageDynamicHandler : BaseDynamicHandler<AnalogRemotePointValueChanged>
    {
        private IStorage<AnalogRemotePoint> storage;

        public AnalogRemotePointStorageDynamicHandler(IStorage<AnalogRemotePoint> storage)
        {
            this.storage = storage;
        }

        protected override void ProcessChanges(AnalogRemotePointValueChanged message)
        {
            Property currentValueProperty = message.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return;
            }

            float value = currentValueProperty.AsFloat();

            AnalogRemotePoint remotePoint = storage.GetEntity(message.Id);

            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {message.Id:X16}. Skipping further processing!");
            }

            remotePoint.Value = value;
        }
    }
}
