using FieldSimulator.PowerSimulator;

namespace FieldSimulator.Modbus.SchemaAligner
{
    public interface IRemotePointSchemaModelAligner
    {
        void AlignRemotePoints(EntityStorage entityStorage);
        void LoadSlaveRemotePoints(SimulatorRemotePoints slaveRemotePoints);
    }
}
