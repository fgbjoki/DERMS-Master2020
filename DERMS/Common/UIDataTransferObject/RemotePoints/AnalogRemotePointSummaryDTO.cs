using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.RemotePoints
{
    [DataContract]
    public class AnalogRemotePointSummaryDTO : RemotePointSummaryDTO
    {
        [DataMember]
        public float Value { get; set; }
    }
}
