using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel
{
    [DataContract]
    public class AnalogMeasurementDTO : MeasurementDTO
    {
        [DataMember]
        public float MinValue { get; set; }
        [DataMember]
        public float MaxValue { get; set; }
    }
}
