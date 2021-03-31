using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.RemotePoints
{
    [DataContract]
    public class AnalogRemotePointSummaryDTO : RemotePointSummaryDTO
    {
        [DataMember]
        public float Value { get; set; }

        [DataMember]
        public float MaxValue { get; set; }

        [DataMember]
        public float MinValue { get; set; }

        [DataMember]
        public AnalogAlarming Alarm { get; set; }
    }
}
