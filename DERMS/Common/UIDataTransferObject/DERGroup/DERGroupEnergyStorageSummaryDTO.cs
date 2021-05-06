using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DERGroup
{
    [DataContract]
    public class DERGroupEnergyStorageSummaryDTO : DistributedEnergyResourceDTO
    {
        [DataMember]
        public float StateOfCharge { get; set; }

        [DataMember]
        public float Capacity { get; set; }
    }
}
