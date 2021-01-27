using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.RemotePoints
{
    [DataContract]
    [KnownType(typeof(AnalogRemotePointSummaryDTO))]
    [KnownType(typeof(DiscreteRemotePointSummaryDTO))]
    public abstract class RemotePointSummaryDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Address { get; set; }
    }
}
