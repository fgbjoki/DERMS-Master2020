using Common.UIDataTransferObject.Schema;

namespace UIAdapter.Schema.StateController
{ 
    public class EquipmentState
    {
        public EquipmentState(long globalId)
        {
            GlobalId = globalId;
        }

        public long GlobalId { get; private set; }
        public bool IsEnergized { get; set; }
        public bool DoesConduct { get; set; }

        public virtual SubSchemaNodeDTO ConvertToDTO()
        {
            return new SubSchemaNodeDTO()
            {
                DoesConduct = DoesConduct,
                GlobalId = GlobalId,
                IsEnergized = IsEnergized
            };
        }
    }

    public class BreakerEquipmentState : EquipmentState
    {
        public BreakerEquipmentState(long globalId) : base(globalId)
        {
        }

        public bool Closed { get; set; }

        public override SubSchemaNodeDTO ConvertToDTO()
        {
            return new SubSchemaBreakerNodeDTO()
            {
                DoesConduct = DoesConduct,
                GlobalId = GlobalId,
                IsEnergized = IsEnergized,
                Closed = Closed
            };
        }
    }
}
