using Common.UIDataTransferObject.RemotePoints;
using Common.UIDataTransferObject.Schema;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject
{
    [DataContract]
    [KnownType(typeof(SubSchemaDTO))]
    [KnownType(typeof(EnergySourceDTO))]
    [KnownType(typeof(SubSchemaNodeDTO))]
    [KnownType(typeof(AnalogRemotePointSummaryDTO))]
    [KnownType(typeof(DiscreteRemotePointSummaryDTO))]
    public abstract class IdentifiedObjectDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
