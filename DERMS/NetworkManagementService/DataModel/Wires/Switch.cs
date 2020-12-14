using NetworkManagementService.DataModel.Core;

namespace NetworkManagementService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        public Switch(long globalId) : base(globalId)
        {

        }

        protected Switch(Switch copyObject) : base(copyObject)
        {

        }
    }
}
