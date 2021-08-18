using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Wires
{
    [DataContract]
    public class Breaker : ProtectedSwitch
    {
        public Breaker(long globalId) : base(globalId)
        {

        }

        protected Breaker(Breaker copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
