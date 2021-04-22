using CalculationEngine.Model.Topology.Transaction;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    class DiscreteRemotePointStorageDynamicHandler : BaseDynamicHandler<DiscreteRemotePointValueChanged>
    {
        private IStorage<DiscreteRemotePoint> discreteStorage;

        public DiscreteRemotePointStorageDynamicHandler(IStorage<DiscreteRemotePoint> discreteStorage)
        {
            this.discreteStorage = discreteStorage;
        }

        protected override void ProcessChanges(DiscreteRemotePointValueChanged message)
        {
            Property currentValueProperty = message.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return;
            }

            int value = currentValueProperty.AsInt();

            DiscreteRemotePoint remotePoint = discreteStorage.GetEntity(message.Id);

            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {message.Id:X16}. Skipping further processing!");
            }

            remotePoint.Value = value;
        }
    }
}
