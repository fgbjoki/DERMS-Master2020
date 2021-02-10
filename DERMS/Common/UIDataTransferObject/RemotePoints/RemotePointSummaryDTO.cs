using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.RemotePoints
{
    [DataContract]
    [KnownType(typeof(AnalogRemotePointSummaryDTO))]
    [KnownType(typeof(DiscreteRemotePointSummaryDTO))]
    public abstract class RemotePointSummaryDTO : IdentifiedObjectDTO
    {
        [DataMember]
        public int Address { get; set; }
    }
}
