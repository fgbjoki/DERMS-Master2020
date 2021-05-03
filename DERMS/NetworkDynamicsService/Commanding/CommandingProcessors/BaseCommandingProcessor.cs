using Common.Communication;
using Common.ComponentStorage;
using Common.SCADA.FieldProcessor;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using NetworkDynamicsService.Model.RemotePoints;

namespace NetworkDynamicsService.Commanding.CommandigProcessors
{
    public abstract class BaseCommandingProcessor<T> : ICommandingProcessor
        where T : RemotePoint
    {
        protected WCFClient<ICommanding> fieldProcessorClient;

        private IStorage<T> storage;

        public BaseCommandingProcessor(IStorage<T> storage)
        {
            this.storage = storage;

            fieldProcessorClient = new WCFClient<ICommanding>("fieldProcessorCommanding");
        }

        public abstract bool ProcessCommand(BaseCommand command);

        protected T GetRemotePoint(long remotePointGlobalId)
        {
            return storage.GetEntity(remotePointGlobalId);
        }
    }
}
