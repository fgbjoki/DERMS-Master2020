using Common.UIDataTransferObject.DERGroup;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject
{
    [DataContract]
    [KnownType(typeof(DERGroupSummaryDTO))]
    [KnownType(typeof(DERGroupGeneratorSummaryDTO))]
    [KnownType(typeof(DERGroupEnergyStorageSummaryDTO))]
    public class DistributedEnergyResourceDTO : IdentifiedObjectDTO
    {
        [DataMember]
        public float ActivePower { get; set; }

        [DataMember]
        public float NominalPower { get; set; }
    }
}
