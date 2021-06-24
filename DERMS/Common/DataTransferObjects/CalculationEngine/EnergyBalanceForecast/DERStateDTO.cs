using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast
{
    [DataContract]
    public class DERStateDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public float ActivePower { get; set; }

        [DataMember]
        public bool IsEnergized { get; set; }
    }
}
