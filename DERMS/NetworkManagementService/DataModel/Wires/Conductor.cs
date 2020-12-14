using NetworkManagementService.DataModel.Core;

namespace NetworkManagementService.DataModel.Wires
{
    public class Conductor : ConductingEquipment
    {
        public Conductor(long globalId) : base(globalId)
        {

        }

        protected Conductor(Conductor copyObject) : base(copyObject)
        {

        }
        
    }
}
