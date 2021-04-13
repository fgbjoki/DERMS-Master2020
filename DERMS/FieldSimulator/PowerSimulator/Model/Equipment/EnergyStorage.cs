namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    class EnergyStorage : ConductingEquipment
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public Generator Generator { get; set; }
    }
}
