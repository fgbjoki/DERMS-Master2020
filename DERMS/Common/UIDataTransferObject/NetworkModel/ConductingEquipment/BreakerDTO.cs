using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel.ConductingEquipment
{
    [DataContract]
    public class BreakerDTO : ConductingEquipmentDTO
    {
        [DataMember]
        public bool NormalOpen { get; set; }
    }
}
