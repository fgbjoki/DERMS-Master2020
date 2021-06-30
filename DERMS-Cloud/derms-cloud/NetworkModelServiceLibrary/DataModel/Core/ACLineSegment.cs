using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Wires
{
    [DataContract]
    public class ACLineSegment : Conductor
    {
        public ACLineSegment(long globalId) : base(globalId)
        {

        }

        protected ACLineSegment(ACLineSegment copyObject) : base(copyObject)
        {

        }
    }
}
