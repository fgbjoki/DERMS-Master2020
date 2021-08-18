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

        [DataMember]
        public float SolarEnergyProduction { get; set; }

        [DataMember]
        public float WindEnergyProduction { get; set; }

        [DataMember]
        public float EnergyStorageEnergyProduction { get; set; }
    }
}
