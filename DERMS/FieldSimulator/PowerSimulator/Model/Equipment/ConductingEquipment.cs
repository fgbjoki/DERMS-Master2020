using FieldSimulator.PowerSimulator.Model.Connectivity;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    public abstract class ConductingEquipment : IdentifiedObject
    {
        public ConductingEquipment(long globalId) : base(globalId)
        {
            Terminals = new List<Terminal>();
        }

        public List<Terminal> Terminals { get; set; }
    }
}
