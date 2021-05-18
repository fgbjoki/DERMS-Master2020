using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.NetworkModel
{
    [DataContract]
    [KnownType(typeof(BreakerDTO))]
    [KnownType(typeof(ConsumerDTO))]
    [KnownType(typeof(SolarPanelDTO))]
    [KnownType(typeof(EnergySourceDTO))]
    [KnownType(typeof(EnergyStorageDTO))]
    [KnownType(typeof(WindGeneratorDTO))]
    [KnownType(typeof(AnalogMeasurementDTO))]
    [KnownType(typeof(DiscreteMeasurementDTO))]
    [KnownType(typeof(ConductingEquipment.DistributedEnergyResourceDTO))]
    public class NetworkModelEntityDTO : IdentifiedObjectDTO
    {
        
    }
}
