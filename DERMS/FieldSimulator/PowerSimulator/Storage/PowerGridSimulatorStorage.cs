using Common.WeatherAPI;
using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser;
using FieldSimulator.PowerSimulator.Storage.Weather;

namespace FieldSimulator.PowerSimulator.Storage
{
    public class PowerGridSimulatorStorage : IRemotePointStorageStorage<float>, IRemotePointStorageStorage<int>
    {
        private IRemotePointStorageStorage<float> analogRemotePointStorage;
        private IRemotePointStorageStorage<int> discreteRemotePointStorage;

        private BreakerTopologyManipulation breakerManipulation;

        private RemotePointValueChangedPublisher publisher;

        public PowerGridSimulatorStorage(BreakerTopologyManipulation breakerManipulation)
        {
            this.breakerManipulation = breakerManipulation;

            WeatherStorage = new WeatherStorage(new WeatherApiClient("3b1ff7b44cbc4a7fa8d124540202911", "Novi Sad"));
        }

        public bool AddItem(RemotePointType remotePointType, ushort address, float newValue)
        {
            return analogRemotePointStorage.AddItem(remotePointType, address, newValue);
        }

        public bool AddItem(RemotePointType remotePointType, ushort address, int newValue)
        {
            breakerManipulation.ChangeBreakerState(address, newValue);
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
            publisher?.ChangeRemotePointValue(remotePointType, address, newValue);
            return analogRemotePointStorage.UpdateValue(remotePointType, address, newValue);
        }

        public bool UpdateValue(RemotePointType remotePointType, ushort address, int newValue)
        {
            breakerManipulation.ChangeBreakerState(address, newValue);
            publisher?.ChangeRemotePointValue(remotePointType, address, newValue);
            return discreteRemotePointStorage.UpdateValue(remotePointType, address, newValue);
        }

        float IRemotePointStorageStorage<float>.GetValue(RemotePointType remotePointType, ushort address)
        {
            return analogRemotePointStorage.GetValue(remotePointType, address);
        }

        public void ReloadStorages()
        {
            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();
        }

        public WeatherStorage WeatherStorage { get; private set; }

        public RemotePointValueChangedPublisher Publisher { set { publisher = value; } }
    }
}
