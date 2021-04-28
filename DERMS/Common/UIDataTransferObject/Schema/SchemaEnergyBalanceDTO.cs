using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Schema
{
    [DataContract]
    public class SchemaEnergyBalanceDTO
    {
        [DataMember]
        public long EnergySourceGid { get; set; }

        [DataMember]
        public float DemandEnergy { get; set; }

        [DataMember]
        public float ImportedEnergy { get; set; }

        [DataMember]
        public float ProducedEnergy { get; set; }
    }
}
