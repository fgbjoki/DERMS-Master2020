using CalculationEngine.Model.Topology.Transaction;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    class DiscreteRemotePointStorageDynamicHandler : BaseDynamicHandler<DiscreteRemotePointValuesChanged>
    {
        private IStorage<DiscreteRemotePoint> discreteStorage;

        public DiscreteRemotePointStorageDynamicHandler(IStorage<DiscreteRemotePoint> discreteStorage)
        {
            this.discreteStorage = discreteStorage;
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

                DiscreteRemotePoint remotePoint = discreteStorage.GetEntity(discreteChange.Id);

                if (remotePoint == null)
                {
                    Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {discreteChange.Id:X16}. Skipping further processing!");
                }

                remotePoint.Value = value;
            }           
        }
    }
}
