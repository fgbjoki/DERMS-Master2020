using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.EnergyBalanceForecast
{
    [DataContract]
    public class DERStateDTO
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public float EnergyUsed { get; set; }

        [DataMember]
        public bool IsEnergized { get; set; }

        [DataMember]
        public float Cost { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
