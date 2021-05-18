using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel.ConductingEquipment
{
    [DataContract]
    [KnownType(typeof(SolarPanelDTO))]
    [KnownType(typeof(EnergyStorageDTO))]
    [KnownType(typeof(WindGeneratorDTO))]
    public class DistributedEnergyResourceDTO : ConductingEquipmentDTO
    {
        [DataMember]
        public float NominalActivePower { get; set; }
    }
}
