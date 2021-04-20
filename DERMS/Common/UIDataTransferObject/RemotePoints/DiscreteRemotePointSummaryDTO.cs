using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.RemotePoints
{
    [DataContract]
    public class DiscreteRemotePointSummaryDTO : RemotePointSummaryDTO
    {
        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public int NormalValue { get; set; }

        [DataMember]
        public int DOMManipulation { get; set; }

        [DataMember]
        public DiscreteAlarming Alarm { get; set; }
    }
}
