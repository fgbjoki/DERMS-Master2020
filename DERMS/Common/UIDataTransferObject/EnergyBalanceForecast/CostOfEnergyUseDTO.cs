using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.EnergyBalanceForecast
{
    [DataContract]
    public class CostOfEnergyUseDTO
    {
        [DataMember]
        public float CostOfEnergyStorageUse { get; set; }

        [DataMember]
        public float CostOfGeneratorShutDown { get; set; }

        [DataMember]
        public float CostOfEnergyImport { get; set; }
    }
}
