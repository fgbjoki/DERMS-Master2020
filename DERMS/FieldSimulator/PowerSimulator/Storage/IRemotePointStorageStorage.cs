using FieldSimulator.Model;

namespace FieldSimulator.PowerSimulator.Storage
{
    public interface IRemotePointStorageStorage<ValueType>
    {
        bool AddItem(RemotePointType remotePointType, ushort address, ValueType newValue);
        ValueType GetValue(RemotePointType remotePointType, ushort address);
        bool UpdateValue(RemotePointType remotePointType, ushort address, ValueType newValue);
        bool EntityExists(RemotePointType remotePointType, ushort address);
    }
}
