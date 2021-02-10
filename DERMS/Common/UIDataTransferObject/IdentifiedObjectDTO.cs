using Common.UIDataTransferObject.RemotePoints;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject
{
    [DataContract]
    [KnownType(typeof(AnalogRemotePointSummaryDTO))]
    [KnownType(typeof(DiscreteRemotePointSummaryDTO))]
    public abstract class IdentifiedObjectDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
