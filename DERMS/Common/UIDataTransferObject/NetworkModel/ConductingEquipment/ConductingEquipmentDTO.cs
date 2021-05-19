using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel.ConductingEquipment
{
    [DataContract]
    [KnownType(typeof(BreakerDTO))]
    [KnownType(typeof(ConsumerDTO))]
    [KnownType(typeof(SolarPanelDTO))]
    [KnownType(typeof(EnergySourceDTO))]
    [KnownType(typeof(EnergyStorageDTO))]
    [KnownType(typeof(WindGeneratorDTO))]
    [KnownType(typeof(DistributedEnergyResourceDTO))]
    public class ConductingEquipmentDTO : NetworkModelEntityDTO
    {
        [DataMember]
        public List<MeasurementDTO> Measurements { get; set; }
    }
}
