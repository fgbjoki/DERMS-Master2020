using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DERGroup
{
    [DataContract]
    public class DERGroupGeneratorSummaryDTO : DistributedEnergyResourceDTO
    {
        [DataMember]
        public GeneratorType GeneratorType { get; set; }
    }
}
