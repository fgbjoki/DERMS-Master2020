using DERMS;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel.ConductingEquipment
{
    [DataContract]
    public class ConsumerDTO : ConductingEquipmentDTO
    {
        [DataMember]
        public float Pfixed { get; set; }
        [DataMember]
        public CustomConsumerType Type { get; set; }
}
}
