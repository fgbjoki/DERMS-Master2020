using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel.ConductingEquipment
{
    [DataContract]
    public class EnergyStorageDTO : DistributedEnergyResourceDTO
    {
        [DataMember]
        public float Capacity { get; set; }
    }
}
