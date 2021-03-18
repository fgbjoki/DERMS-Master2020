namespace CalculationEngine.Model.Topology.Transaction
{
    public class Generator : ConductingEquipment
    {
        public Generator(long globalId) : base(globalId)
        {
        }

        public EnergyStorage EnergyStorage { get; private set; }

        public void ConnectToEnergyStorage(EnergyStorage energyStorage)
        {
            EnergyStorage = energyStorage;
            energyStorage.Generator = this;
        }
    }
}
