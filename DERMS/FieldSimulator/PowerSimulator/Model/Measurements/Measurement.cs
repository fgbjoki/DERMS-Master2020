using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator.Model.Measurements
{
    public abstract class Measurement : IdentifiedObject
    {
        public Measurement(long globalId) : base(globalId)
        {
        }

        public ConductingEquipment ConductingEquipment { get; set; }

        public int Address { get; set; }
    }
}
