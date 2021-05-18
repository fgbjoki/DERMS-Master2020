using Common.UIDataTransferObject.DERGroup;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using Common.UIDataTransferObject.RemotePoints;
using Common.UIDataTransferObject.Schema;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject
{
    [DataContract]
    [KnownType(typeof(SubSchemaDTO))]
    [KnownType(typeof(MeasurementDTO))]
    [KnownType(typeof(Schema.EnergySourceDTO))]
    [KnownType(typeof(SubSchemaNodeDTO))]
    [KnownType(typeof(DERGroupSummaryDTO))]
    [KnownType(typeof(AnalogMeasurementDTO))]
    [KnownType(typeof(DiscreteMeasurementDTO))]
    [KnownType(typeof(SubSchemaBreakerNodeDTO))]
    [KnownType(typeof(DERGroupGeneratorSummaryDTO))]
    [KnownType(typeof(AnalogRemotePointSummaryDTO))]
    [KnownType(typeof(NetworkModel.ConductingEquipment.DistributedEnergyResourceDTO))]
    [KnownType(typeof(DiscreteRemotePointSummaryDTO))]
    [KnownType(typeof(DERGroupEnergyStorageSummaryDTO))]
    public class IdentifiedObjectDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
