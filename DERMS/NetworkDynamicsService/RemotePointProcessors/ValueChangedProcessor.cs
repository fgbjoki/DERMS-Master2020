using Common.SCADA;
using Common.ComponentStorage;
using NetworkDynamicsService.Model.RemotePoints;
using NServiceBus;
using Common.GDA;
using Common.PubSub;
using System.Threading;
using Common.Logger;

namespace NetworkDynamicsService.RemotePointProcessors
{
    public abstract class ValueChangedProcessor<RemotePointType> : IValueChangedProcessor
        where RemotePointType : RemotePoint
    {
        private ReaderWriterLockSlim locker;
        private IStorage<RemotePointType> storage;
        private IDynamicPublisher publisher;

        public ValueChangedProcessor(IStorage<RemotePointType> storage, IDynamicPublisher publisher)
        {
            this.storage = storage;
            this.publisher = publisher;

            locker = new ReaderWriterLockSlim();
        }

        public void ProcessChangedValue(RemotePointFieldValue rawValue)
        {
            locker.EnterReadLock();
            RemotePointType remotePoint = storage.GetEntity(rawValue.GlobalId);
            locker.ExitReadLock();

            if (remotePoint == null)
            {
                Logger.Instance.Log($"[{GetType()}] Coudln't find remote point to process with gid: 0x{rawValue.GlobalId:X16}.");
                return;
            }

            if (!HasValueChanged(remotePoint, rawValue.Value))
            {
                return;
            }

            locker.EnterWriteLock();
            ResourceDescription changes = ApplyChanges(remotePoint, rawValue.Value);
            locker.ExitWriteLock();

            // TODO UNCOMMENT THIS WHEN IMPLEMENTING DATABASE MANIPULATION
            //SaveChanges(remotePoint);

            IEvent publication = GetPublication(changes);

            PublishChanges(publication);
        }

        /// <summary>
        /// Applies all needed changes on <paramref name="remotePoint"/>.
        /// </summary>
        /// <returns><b>True</b> if the remote point was changes, otherwise <b>false</b>.</returns>
        protected abstract bool HasValueChanged(RemotePointType remotePoint, ushort rawValue);

        protected abstract ResourceDescription ApplyChanges(RemotePointType remotePoint, ushort rawValue);

        protected abstract IEvent GetPublication(ResourceDescription changes);

        private void PublishChanges(IEvent publication)
        {
            publisher.Publish(publication);
        }

        // TODO UNCOMMENT THIS WHEN IMPLEMENTING DATABASE MANIPULATION
        //protected abstract void SaveChanges(RemotePointType remotePoint);
    }
}
