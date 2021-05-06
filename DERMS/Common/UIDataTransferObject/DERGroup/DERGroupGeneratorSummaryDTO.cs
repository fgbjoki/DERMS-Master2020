using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DERGroup
{
    [DataContract]
    public class DERGroupGeneratorSummaryDTO : DistributedEnergyResourceDTO
    {
        [DataMember]
        public GeneratorType GeneratorType { get; set; }

        [DataMember]
        public float StartUpSpeed { get; set; }

        [DataMember]
        public float NominalSpeed { get; set; }

        [DataMember]
        public float CutOutSpeed { get; set; }
    }
}
