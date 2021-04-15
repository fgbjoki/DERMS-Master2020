using FieldSimulator.Model;

namespace FieldSimulator.Modbus.SchemaAligner
{
    public struct SimulatorRemotePoints
    {
        public CoilWrapper[] Coils { get; set; }
        public DiscreteInputWrapper[] DiscreteInput { get; set; }
        public InputRegisterWrapper[] InputRegisters { get; set; }
        public HoldingRegisterWrapper[] HoldingRegisters { get; set; }
    }
}
