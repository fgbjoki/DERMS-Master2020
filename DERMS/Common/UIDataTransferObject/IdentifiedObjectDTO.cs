using Common.UIDataTransferObject.DERGroup;
using Common.UIDataTransferObject.RemotePoints;
using Common.UIDataTransferObject.Schema;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject
{
    [DataContract]
    [KnownType(typeof(SubSchemaDTO))]
    [KnownType(typeof(EnergySourceDTO))]
    [KnownType(typeof(SubSchemaNodeDTO))]
    [KnownType(typeof(DERGroupSummaryDTO))]
    [KnownType(typeof(SubSchemaBreakerNodeDTO))]
    [KnownType(typeof(DERGroupGeneratorSummaryDTO))]
    [KnownType(typeof(AnalogRemotePointSummaryDTO))]
    [KnownType(typeof(DiscreteRemotePointSummaryDTO))]
    [KnownType(typeof(DERGroupEnergyStorageSummaryDTO))]
    public abstract class IdentifiedObjectDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
