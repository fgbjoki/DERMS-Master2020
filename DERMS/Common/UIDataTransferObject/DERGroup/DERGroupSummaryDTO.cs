using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DERGroup
{
    [DataContract]
    public class DERGroupSummaryDTO : DistributedEnergyResourceDTO
    {
        [DataMember]
        public DERGroupGeneratorSummaryDTO Generator { get; set; }

        [DataMember]
        public DERGroupEnergyStorageSummaryDTO EnergyStorage { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public long LocationGid { get; set; }
    }
}
