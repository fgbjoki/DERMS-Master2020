using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel.ConductingEquipment
{
    [DataContract]
    public class WindGeneratorDTO : DistributedEnergyResourceDTO
    {
        [DataMember]
        public float StartUpSpeed { get; set; }
        [DataMember]
        public float CutOutSpeed { get; set; }
        [DataMember]
        public float NominalSpeed { get; set; }
    }
}
