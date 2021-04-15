using FieldSimulator.Model;

namespace FieldSimulator.PowerSimulator.Storage
{
    public class PowerGridSimulatorStorage : IRemotePointStorageStorage<float>, IRemotePointStorageStorage<int>
    {
        private IRemotePointStorageStorage<float> analogRemotePointStorage;
        private IRemotePointStorageStorage<int> discreteRemotePointStorage;

        public PowerGridSimulatorStorage()
        {

        }

        public bool AddItem(RemotePointType remotePointType, ushort address, float newValue)
        {
            return analogRemotePointStorage.AddItem(remotePointType, address, newValue);
        }

        public bool AddItem(RemotePointType remotePointType, ushort address, int newValue)
        {
            return discreteRemotePointStorage.AddItem(remotePointType, address, newValue);
        }

        public bool EntityExists(RemotePointType remotePointType, ushort address)
        {
            return analogRemotePointStorage.EntityExists(remotePointType, address);
        }

        public int GetValue(RemotePointType remotePointType, ushort address)
        {
            return discreteRemotePointStorage.GetValue(remotePointType, address);
        }

        public bool UpdateValue(RemotePointType remotePointType, ushort address, float newValue)
        {
            return analogRemotePointStorage.UpdateValue(remotePointType, address, newValue);
        }

        public bool UpdateValue(RemotePointType remotePointType, ushort address, int newValue)
        {
            return discreteRemotePointStorage.UpdateValue(remotePointType, address, newValue);
        }

        float IRemotePointStorageStorage<float>.GetValue(RemotePointType remotePointType, ushort address)
        {
            return analogRemotePointStorage.GetValue(remotePointType, address);
        }

        public void Clear()
        {
            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();
        }
    }
}
