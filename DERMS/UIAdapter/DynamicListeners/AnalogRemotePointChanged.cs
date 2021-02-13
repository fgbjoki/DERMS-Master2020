using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using NServiceBus;
using System.Threading.Tasks;
using UIAdapter.Model;

namespace UIAdapter.DynamicListeners
{
    public class AnalogRemotePointChangedHandler : IHandleMessages<ResourceDescription>
    {
        private IStorage<AnalogRemotePoint> storage;

        public Task Handle(ResourceDescription message, IMessageHandlerContext context)
        {
            if (message == null)
            {
                // log
                return Task.CompletedTask;
            }

            float value = message.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();

            AnalogRemotePoint remotePoint = storage.GetEntity(message.Id);

            if (remotePoint == null)
            {
                // log
                return Task.CompletedTask;
            }

            remotePoint.Value = value;
            return Task.CompletedTask;
        }
    }
}
