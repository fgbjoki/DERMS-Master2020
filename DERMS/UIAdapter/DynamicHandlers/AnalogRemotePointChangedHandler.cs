using Common.AbstractModel;
using Common.ComponentStorage;
using Common.Logger;
using Common.PubSub.Messages;
using NServiceBus;
using System.Threading.Tasks;
using UIAdapter.Model;

namespace UIAdapter.DynamicHandlers
{
    public class AnalogRemotePointChangedHandler : IHandleMessages<AnalogRemotePointValueChanged>
    {
        private IStorage<AnalogRemotePoint> storage;

        public AnalogRemotePointChangedHandler(IStorage<AnalogRemotePoint> storage)
        {
            this.storage = storage;
        }

        public Task Handle(AnalogRemotePointValueChanged message, IMessageHandlerContext context)
        {
            if (message == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] There was no data published. Skipping further processing!");
                return Task.CompletedTask;
            }

            float value = message.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();

            AnalogRemotePoint remotePoint = storage.GetEntity(message.Id);

            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{this.GetType()}] Couldn't find remote point with gid {message.Id:X16}. Skipping further processing!");
                return Task.CompletedTask;
            }

            remotePoint.Value = value;
            return Task.CompletedTask;
        }
    }
}
