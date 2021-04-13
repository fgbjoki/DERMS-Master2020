using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator.Model.Connectivity
{
    public class Terminal : IdentifiedObject
    {
        public Terminal(long globalId) : base(globalId)
        {
        }

        public ConductingEquipment ConductingEquipment { get; set; }

        public ConnectivityNode ConnectivityNode { get; set; }
    }
}
