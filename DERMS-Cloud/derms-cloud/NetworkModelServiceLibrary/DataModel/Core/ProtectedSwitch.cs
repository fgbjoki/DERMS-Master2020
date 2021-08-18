using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Wires
{
    [DataContract]
    public class ProtectedSwitch : Switch
    {
        public ProtectedSwitch(long globalId) : base(globalId)
        {

        }

        protected ProtectedSwitch(ProtectedSwitch copyObject) : base(copyObject)
        {

        }
    }
}
