using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.Model;

namespace UIAdapter.PubSub.DynamicHandlers
{
    public class DiscreteRemotePointStorageDynamicHandler : BaseDynamicHandler<DiscreteRemotePointValuesChanged>
    {
        private IStorage<DiscreteRemotePoint> storage;

        public DiscreteRemotePointStorageDynamicHandler(IStorage<DiscreteRemotePoint> storage)
        {
            this.storage = storage;
        }

        protected override void ProcessChanges(DiscreteRemotePointValuesChanged message)
        {
            foreach (var discreteChange in message)
            {
                ProcessValue(discreteChange);
                ProcessDOM(discreteChange);
            }
        }

        private void ProcessValue(ResourceDescription discreteChange)
        {
            Property currentValueProperty = discreteChange.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return;
            }

            int value = currentValueProperty.AsInt();

            DiscreteRemotePoint remotePoint = storage.GetEntity(discreteChange.Id);

            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {discreteChange.Id:X16}. Skipping further processing!");
            }

            remotePoint.Value = value;
        }

        private void ProcessDOM(ResourceDescription discreteChange)
        {
            Property currentValueProperty = discreteChange.GetProperty(ModelCode.MEASUREMENTDISCRETE_DOM);

            if (currentValueProperty == null)
            {
                return;
            }

            int domManipulationValue = currentValueProperty.AsInt();

            DiscreteRemotePoint remotePoint = storage.GetEntity(discreteChange.Id);

            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {discreteChange.Id:X16}. Skipping further processing!");
            }

            remotePoint.DOMManipulation = domManipulationValue;
        }
    }
}
