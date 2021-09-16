using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.EnergyBalanceForecast
{
    [DataContract]
    public class DomainParametersDTO
    {
        [DataMember]
        public float CostOfImportedEnergyPerKWH { get; set; }
        [DataMember]
        public float CostOfEnergyStorageUsePerKWH { get; set; }
        [DataMember]
        public float CostOfGeneratorShutdownPerKWH { get; set; }

        [DataMember]
        /// <summary>
        /// Simulation interval used for calculating energy uses/losses. Time is represented in minutes.
        /// </summary>
        public ulong SimulationInterval { get; set; }

        [DataMember]
        /// <summary>
        /// Maximum seconds for algorithm to compute.
        /// </summary>
        public double CalculatingTime { get; set; }
    }
}
