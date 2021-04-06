using Common.AbstractModel;
using Common.ComponentStorage;
using Common.DynamicMessages;
using NServiceBus;
using System.Threading.Tasks;
using UIAdapter.Model;

namespace UIAdapter.DynamicHandlers
{
    public class DiscreteRemotePointChangedHandler : IHandleMessages<DiscreteRemotePointValueChanged>
    {
        private IStorage<DiscreteRemotePoint> storage;

        public DiscreteRemotePointChangedHandler(IStorage<DiscreteRemotePoint> storage)
        {
            this.storage = storage;
        }

        public Task Handle(DiscreteRemotePointValueChanged message, IMessageHandlerContext context)
        {
            if(message == null)
            {
                // log
                return Task.CompletedTask;
            }

            int value = message.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE).AsInt();

            DiscreteRemotePoint remotePoint = storage.GetEntity(message.Id);

            if(remotePoint == null)
            {
                // log
                return Task.CompletedTask;
            }

            remotePoint.Value = value;
            return Task.CompletedTask;
        }
    }
}
