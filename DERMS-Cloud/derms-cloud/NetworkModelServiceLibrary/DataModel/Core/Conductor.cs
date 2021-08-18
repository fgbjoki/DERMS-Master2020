using NetworkManagementService.DataModel.Core;
using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Wires
{
    [DataContract]
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
