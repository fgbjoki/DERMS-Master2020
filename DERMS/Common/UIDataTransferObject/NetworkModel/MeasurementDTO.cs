using Common.AbstractModel;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel
{
    [DataContract]
    [KnownType(typeof(AnalogMeasurementDTO))]
    [KnownType(typeof(DiscreteMeasurementDTO))]
    public class MeasurementDTO : NetworkModelEntityDTO
    {
        [DataMember]
        public SignalDirection SignalDirection { get; set; }

        [DataMember]
        public MeasurementType MeasurementType { get; set; }

        [DataMember]
        public int Address { get; set; }
    }
}
