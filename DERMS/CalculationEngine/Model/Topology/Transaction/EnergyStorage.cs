namespace CalculationEngine.Model.Topology.Transaction
{
    public class EnergyStorage : ConductingEquipment
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public ConductingEquipment Generator { get; set; }
    }
}
