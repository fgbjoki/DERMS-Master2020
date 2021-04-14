using FieldSimulator.PowerSimulator.Model.Connectivity;
using FieldSimulator.PowerSimulator.Model.Measurements;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    public abstract class ConductingEquipment : IdentifiedObject
    {
        public ConductingEquipment(long globalId) : base(globalId)
        {
            Terminals = new List<Terminal>();
            Measurements = new List<Measurement>();
        }

        public List<Terminal> Terminals { get; set; }

        public List<Measurement> Measurements { get; set; }
    }
}
